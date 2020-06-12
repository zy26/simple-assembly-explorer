
@echo off
echo Cleaning ...

call deldir ILSpy\ICSharpCode.Decompiler\obj
call deldir ILSpy\ICSharpCode.Decompiler\bin

call deldir ILSpy\NRefactory\ICSharpCode.NRefactory\obj
call deldir ILSpy\NRefactory\ICSharpCode.NRefactory\bin

call deldir Mono.Cecil\obj
call deldir Mono.Cecil\bin

call deldir SAE.Deobf9RayHelper\obj 
call deldir SAE.Deobf9RayHelper\bin

call deldir SAE.EditFile\obj 
call deldir SAE.EditFile\bin

call deldir SAE.FileDisassembler\obj 
call deldir SAE.FileDisassembler\bin

call deldir SAE.ILMerge\obj 
call deldir SAE.ILMerge\bin

call deldir SAE.MethodSearcher\obj 
call deldir SAE.MethodSearcher\bin

call deldir SAE.PluginSample\obj
call deldir SAE.PluginSample\bin

call deldir SAE.Reflector\obj 
call deldir SAE.Reflector\bin

call deldir SimpleAssemblyExplorer.Core\obj
call deldir SimpleAssemblyExplorer.Core\bin

call deldir SimpleAssemblyExplorer.plugin\obj
call deldir SimpleAssemblyExplorer.plugin\bin

call deldir SimpleProfiler\Debug 
call deldir SimpleProfiler\Release
call deldir SimpleProfiler\Win32
call deldir SimpleProfiler\x64
call delfile SimpleProfiler\SimpleProfiler.ncb

call deldir SimpleUtils\obj
call deldir SimpleUtils\bin

call deldir TestProject\obj
call deldir TestProject\bin
call delfile TestProject\Assembly\HWISD_nat.dll
call delfile TestProject\Temp\*.*

call deldir TestSample\obj
call deldir TestSample\bin


call delfile SimpleAssemblyExplorer.ncb

call deldir SimpleAssemblyExplorer\obj
call deldir SimpleAssemblyExplorer\bin\x86
call deldir SimpleAssemblyExplorer\bin\x64

call delfile SimpleAssemblyExplorer\bin\debug\*.*
call delfile SimpleAssemblyExplorer\bin\debug\Plugins\*.*

call deldummy SimpleAssemblyExplorer\bin\Release

echo.
echo Checking Version ...

support\AppVer SimpleAssemblyExplorer\bin\release\SimpleAssemblyExplorer.exe sae.autoupdate.xml

echo.
pause