using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleAssemblyExplorer;
using SimpleAssemblyExplorer.LutzReflector;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace TestProject
{
    
    /// <summary>
    ///This is a test class for DeobfuscatorTest and is intended
    ///to contain all DeobfuscatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReflectorTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        /// Notes:
        /// 1) Completely full test, no obfuscated comment
        /// 2) Foreach statement should be handled properly, no "using (enumerator ="
        /// 3) SmartAssembly global exception handler should be removed in second Go
        /// </summary>
        [TestMethod()]
        public void Reflector65_FullTest()
        {
            TestDeobfOptions options = new TestDeobfOptions();
            options.Rows = new string[] { "Reflector.6.5.exe" };
            options.ApplyFrom("Name, String and Boolean Function");
            TestDeobfuscator deobf = new TestDeobfuscator(options);
            deobf.Go();

            string outputFile = options.OutputFiles[0];
            TestDeobfOptions options2 = new TestDeobfOptions();
            options2.SourceDir = Path.GetDirectoryName(outputFile);
            options2.Rows = new string[] { outputFile };
            options2.ApplyFrom("Flow without Boolean Function");            
            options2.LoopCount = 3;
            //options2.chkReflectorFixChecked = true;
            options2.ExceptionHandlerFile = new ExceptionHandlerFile(ExceptionHandlerFile.Default.FileName);
            options2.ExceptionHandlerFile.Keywords.Add("NS006.c000021::m000");            
            TestDeobfuscator deobf2 = new TestDeobfuscator(options2);
            deobf2.Go();

            string outputFile2 = options2.OutputFiles[0];
            TestUtils.CheckAndOutput(new string[] { outputFile2 },
                new string[] { TestUtils.ObfuscatedCommentText, TestUtils.ObfuscatedForeachText }
                );            

            Utils.DeleteFile(outputFile);
            Utils.DeleteFile(outputFile2);
        }


        /// <summary>
        /// Notes:
        /// 1) Completely full test, no obfuscated comment
        /// 2) Foreach statement should be handled properly, no "using (enumerator ="
        /// 3) SmartAssembly global exception handler should be removed in second Go
        /// 4) Delegate Call removed
        /// </summary>
        [TestMethod()]
        public void Reflector70_FullTest()
        {
            TestDeobfOptions options = new TestDeobfOptions();
            options.Rows = new string[] { "Reflector.7.0.exe" };            
            options.ApplyFrom("Default");
            options.LoopCount = 2;
            options.chkDelegateCallChecked = true;
            options.IgnoredMethodFile.Regexes.Add(new Regex("0x060000c3")); //for 7.0
            TestDeobfuscator deobf = new TestDeobfuscator(options);
            deobf.Go();

            string outputFile = options.OutputFiles[0];
            TestDeobfOptions options2 = new TestDeobfOptions();
            options2.SourceDir = Path.GetDirectoryName(outputFile);
            options2.Rows = new string[] { outputFile };
            options2.ApplyFrom("Flow without Boolean Function");
            options2.LoopCount = 2;
            options2.chkReflectorFixChecked = true;
            options2.ExceptionHandlerFile = new ExceptionHandlerFile(ExceptionHandlerFile.Default.FileName);
            options2.ExceptionHandlerFile.Keywords.Add("NS011.c000065::m000");
            TestDeobfuscator deobf2 = new TestDeobfuscator(options2);
            deobf2.Go();

            string outputFile2 = options2.OutputFiles[0];
            List<MethodDeclarationInfo> list = TestUtils.FindMethods(
                new string[] { outputFile2 },
                new string[] { TestUtils.ObfuscatedCommentText, TestUtils.ObfuscatedForeachText },
                null);

            int unexpected = 0;
            int expected = 0;
            foreach (MethodDeclarationInfo mdi in list)
            {
                switch (mdi.MethodDeclaration.Token)
                {
                    //case 0x060008ef:
                        //c000183.m0003ea(Int32) : Int32 (0x060008ef):
                        //use cond branch (up) once at last
                        //expected++;
                        //break;

                    default:
                        TestUtils.Output(mdi, null);
                        unexpected++;
                        break;
                }
            }

            TestUtils.AssertFail(0, expected, unexpected);

            Utils.DeleteFile(outputFile);
            Utils.DeleteFile(outputFile2);
        }

        [TestMethod()]
        public void Reflector74_FullTest()
        {
            //ensure to load default reflector at first
            SimpleReflector dummy = SimpleReflector.Default;

            TestDeobfOptions options = new TestDeobfOptions();
            options.Rows = new string[] { "Reflector.7.4.exe" };
            options.ApplyFrom("Default");
            options.LoopCount = 2;
            options.chkDelegateCallChecked = true;
            options.IgnoredMethodFile.Regexes.Add(new Regex("RedGate.Licensing"));
            options.IgnoredMethodFile.Regexes.Add(new Regex("ReflectorCustomDialog"));            
            TestDeobfuscator deobf = new TestDeobfuscator(options);
            deobf.Go();

            string outputFile = options.OutputFiles[0];
            TestDeobfOptions options2 = new TestDeobfOptions();
            options2.SourceDir = Path.GetDirectoryName(outputFile);
            options2.Rows = new string[] { outputFile };
            options2.ApplyFrom("Flow without Boolean Function");
            options2.LoopCount = 2;
            options2.chkReflectorFixChecked = true;
            options2.ExceptionHandlerFile = new ExceptionHandlerFile(ExceptionHandlerFile.Default.FileName);
            options2.ExceptionHandlerFile.Keywords.Add("NS011.c000065::m000");
            TestDeobfuscator deobf2 = new TestDeobfuscator(options2);
            deobf2.Go();

            string outputFile2 = options2.OutputFiles[0];
            List<MethodDeclarationInfo> list = TestUtils.FindMethods(
                new string[] { outputFile2 },
                new string[] { TestUtils.ObfuscatedCommentText, TestUtils.ObfuscatedForeachText },
                null);

            int unexpected = 0;
            int expected = 0;
            foreach (MethodDeclarationInfo mdi in list)
            {
                switch (mdi.MethodDeclaration.Token)
                {
                    //Edit 2012-12-26: fixed in .Net Reflector 7.7
                    //case 0x060019ee:
                    //case 0x0600195b:
                        //TODO: find reason? or use ILSpy                        
                        //Non-matching stack heights
                        //expected++;
                        //break;
                    default:
                        TestUtils.Output(mdi, null);
                        unexpected++;
                        break;
                }
            }

            TestUtils.AssertFail(0, expected, unexpected);

            Utils.DeleteFile(outputFile);
            Utils.DeleteFile(outputFile2);
        }

        /*
        /// <summary>
        /// Want to show detail message of exception for TranslateMethodDeclaration
        /// Seems different version has different exception
        /// </summary>
        [TestMethod()]
        public void Reflector_InjectComment()
        {
            //search target type
            AssemblyDefinition ad = AssemblyDefinition.ReadAssembly(Global.Reflector);
            TypeDefinition targetType = TestUtils.FindType(ad, "0x02000431");
            Assert.IsNotNull(targetType, "Cannot find target type.");

            //search target method
            MethodDefinition targetMethod = TestUtils.FindMethod(targetType, "TranslateMethodDeclaration",
                new string[] { "Reflector.CodeModel.IMethodDeclaration", "Reflector.CodeModel.IMethodBody" },
                null);
            Assert.IsNotNull(targetMethod, "Cannot find target method.");

            //search comment related types
            TypeDefinition iCommentStatementType = TestUtils.FindType(ad, "Reflector.CodeModel.ICommentStatement");
            Assert.IsNotNull(iCommentStatementType, "Cannot find type: Reflector.CodeModel.ICommentStatement.");
            
            TypeDefinition iCommentType = TestUtils.FindType(ad, "Reflector.CodeModel.IComment");
            Assert.IsNotNull(iCommentType, "Cannot find type: Reflector.CodeModel.IComment.");

            TypeDefinition commentStatementType = TestUtils.FindType(ad, "Reflector.CodeModel.Memory.CommentStatement");
            Assert.IsNotNull(commentStatementType, "Cannot find type: Reflector.CodeModel.Memory.CommentStatement.");

            TypeDefinition commentType = TestUtils.FindType(ad, "Reflector.CodeModel.Memory.Comment");
            Assert.IsNotNull(commentType, "Cannot find type: Reflector.CodeModel.Memory.Comment.");

            TypeDefinition iBlockStatementType = TestUtils.FindType(ad, "Reflector.CodeModel.IBlockStatement");
            Assert.IsNotNull(iBlockStatementType, "Cannot find type: Reflector.CodeModel.IBlockStatement.");

            TypeDefinition iStatementCollectionType = TestUtils.FindType(ad, "Reflector.CodeModel.IStatementCollection");
            Assert.IsNotNull(iStatementCollectionType, "Cannot find type: Reflector.CodeModel.IStatementCollection.");


            //import System.Exception
            System.Type exType = System.Type.GetType("System.Exception", false, true);
            System.Reflection.MethodInfo exGetMessageInfo = exType.GetMethod("get_Message");
            TypeReference exTr = targetType.Module.Import(exType);
            MethodReference exGetMessage = targetType.Module.Import(exGetMessageInfo);

            #region inject new method CreateExceptionStatement
            //create method
            MethodDefinition createExceptionStatementMethod = new MethodDefinition("CreateExceptionStatement",
                MethodAttributes.Static | MethodAttributes.Private | MethodAttributes.HideBySig,
                iCommentStatementType);

            //create parameter
            ParameterDefinition pd = new ParameterDefinition("ex",
                 ParameterAttributes.None,
                 exTr);
            createExceptionStatementMethod.Parameters.Add(pd);

            //create variables
            createExceptionStatementMethod.Body.Variables.Add(new VariableDefinition("statement", iCommentStatementType));

            //add method to target type
            targetType.Methods.Add(createExceptionStatementMethod);

            //add instructions 
            MethodBody body = createExceptionStatementMethod.Body;
            Collection<Instruction> ic = body.Instructions;
            ic.Add(Instruction.Create(OpCodes.Newobj, TestUtils.FindConstructor(commentStatementType)));
            ic.Add(Instruction.Create(OpCodes.Stloc_0));
            ic.Add(Instruction.Create(OpCodes.Ldloc_0));
            ic.Add(Instruction.Create(OpCodes.Newobj, TestUtils.FindConstructor(commentType)));
            ic.Add(Instruction.Create(OpCodes.Callvirt, TestUtils.FindMethod(iCommentStatementType, "set_Comment", new string[] { "Reflector.CodeModel.IComment" }, null)));
            ic.Add(Instruction.Create(OpCodes.Ldloc_0));
            ic.Add(Instruction.Create(OpCodes.Callvirt, TestUtils.FindMethod(iCommentStatementType, "get_Comment", null, "Reflector.CodeModel.IComment")));
            ic.Add(Instruction.Create(OpCodes.Ldarg_0));
            ic.Add(Instruction.Create(OpCodes.Brfalse_S, ic[0])); //wrong operand 1
            ic.Add(Instruction.Create(OpCodes.Ldarg_0));
            ic.Add(Instruction.Create(OpCodes.Callvirt, exGetMessage));
            ic.Add(Instruction.Create(OpCodes.Br_S, ic[0])); //wrong operand 2
            ic.Add(Instruction.Create(OpCodes.Ldstr, "Exception is null"));
            
            Assert.AreEqual(OpCodes.Brfalse_S, ic[ic.Count - 5].OpCode, "Unexpected opcode found.");
            ic[ic.Count - 5].Operand = ic[ic.Count - 1]; //fix operand 1

            ic.Add(Instruction.Create(OpCodes.Callvirt, TestUtils.FindMethod(iCommentType, "set_Text", new string[] { "System.String" }, null)));

            Assert.AreEqual(OpCodes.Br_S, ic[ic.Count - 3].OpCode, "Unexpected opcode found.");
            ic[ic.Count - 3].Operand = ic[ic.Count - 1]; //fix operand 2

            ic.Add(Instruction.Create(OpCodes.Ldloc_0));
            ic.Add(Instruction.Create(OpCodes.Ret));
            #endregion inject new method CreateExceptionStatement

            #region inject comment to TranslateMethodDeclaration

            VariableDefinition blockStatement = targetMethod.Body.Variables[2];
            Assert.AreEqual(blockStatement.VariableType.FullName, "Reflector.CodeModel.IBlockStatement", "Unexpected variable type.");

            //add exception local variable
            VariableDefinition injectExVd = new VariableDefinition("injectEx", exTr);
            targetMethod.Body.Variables.Add(injectExVd);

            ic = targetMethod.Body.Instructions;
            foreach (ExceptionHandler eh in targetMethod.Body.ExceptionHandlers)
            {
                if(eh.HandlerType != ExceptionHandlerType.Catch)
                    continue;

                if (eh.CatchType != exTr && eh.HandlerStart.OpCode.Code == Code.Pop)
                {
                    eh.CatchType = exTr;
                    eh.HandlerStart.OpCode = OpCodes.Stloc;
                    eh.HandlerStart.Operand = injectExVd;

                    Instruction ins = eh.HandlerStart.Next;
                    Instruction insEnd = eh.HandlerEnd;
                    while (ins != null && ins.Offset < insEnd.Offset)
                    {
                        if (ins.OpCode.Code == Code.Callvirt && 
                            ins.Operand.ToString() == "System.Void Reflector.CodeModel.IStatementCollection::Insert(System.Int32,Reflector.CodeModel.IStatement)"
                            )
                        {
                            Instruction prev = ins.Previous;
                            if (prev.OpCode.OperandType == OperandType.InlineVar || prev.OpCode.OperandType == OperandType.ShortInlineVar)
                            {
                                prev = prev.Previous;
                                if (prev.OpCode.Code == Code.Ldc_I4_0)
                                {
                                    int index = ic.IndexOf(ins) + 1;
                                    ic.Insert(index, Instruction.Create(OpCodes.Callvirt, TestUtils.FindMethod(iStatementCollectionType, "Insert", new string[] { "System.Int32", "Reflector.CodeModel.IStatement" }, null)));
                                    ic.Insert(index, Instruction.Create(OpCodes.Call, createExceptionStatementMethod));
                                    ic.Insert(index, Instruction.Create(OpCodes.Ldloc_S, injectExVd));
                                    ic.Insert(index, Instruction.Create(OpCodes.Ldc_I4_1));
                                    ic.Insert(index, Instruction.Create(OpCodes.Callvirt, TestUtils.FindMethod(iBlockStatementType, "get_Statements", null, "Reflector.CodeModel.IStatementCollection")));
                                    ic.Insert(index, Instruction.Create(OpCodes.Ldloc_2));
                                    ins = ic[index + 5];
                                }
                            }
                        }
                        ins = ins.Next;
                    }

                }
            }

            #endregion inject comment to TranslateMethodDeclaration

            //save assembly
            string outputFile = Path.Combine(Global.TempDir, "Reflector.Ex.exe");
            ad.Write(outputFile);

            //sign assembly
            TestStrongNameOptions sno = new TestStrongNameOptions();
            sno.Rows = new string[] { outputFile };
            sno.rbSignChecked = true;
            TestStrongNamer sn = new TestStrongNamer(sno);
            sn.Go();
        }

         */

    }//end of class
}
