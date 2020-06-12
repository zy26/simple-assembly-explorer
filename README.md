# Write a deobfuscator plugin for SAE #

**Steps:**

1. Create a Class Library project, e.g. `SAE.MyDeobfPlugin`

It's recommended to keep assembly file name and name space same.

2. Add a reference to `SimpleAssemblyExplorer.Plugin.dll`.

This assembly can be found in SAE archive file.

3. Add a new class and inherit DefaultDeobfPlugin

SAE will firstly search all exported types for plugin, if error occured, will secondly try to use assembly file name + DeobfPlugin to search plugin (i.e. `SAE.MyDeobfPlugin.DeobfPlugin`).

And, you need to have at least a constructor with a IHost argument.

4. Override PluginInfo property and the methods you wish to handle

5. Build and put the assembly to SAE's Plugins directory

6. Open SAE to try the plugin

**Samples:**

You can get below SAE deobfuscator plugins source code from repository for reference:
  * `SAE.DeobfPluginSample`

# Introduction #

**Simple Assembly Explorer (SAE)** is an **OPEN SOURCE** .Net assembly tool.

# Details #

**Features:**
  * Assembler: call ilasm to assemble il file
  * Disassembler: call ildasm to disassemble assembly
  * Deobfuscator: de-obfuscate obfuscated assembly
  * Strong Name: remove strong name, sign assembly, add/remove assembly to/from GAC
  * PE Verify: call peverify to verify assemblies

  * Class Editor: browse/view assembly classes, edit method instructions, manipulate resources
  * Run Method: run static methods
  * Profiler: Trace function calls and parameters with SimpleProfiler

  * Relector: plugin which call Reflector to browse selected assembly
  * Editor: plugin which call an editor to edit selected assembly
  * ILMerge: plugin which call ILMerge to merge selected assemblies
  * Plugin Sample: simple plugin sample

  * Copy Info: copy information of selected assemblies to clipboard
  * Open Folder: open container folder
  * Delete File: delete selected files

**Installation:**
  1. Install .Net Framework 4.0
  1. Install .Net Framework 4.0 SDK
  1. Extract to any directory

**Usage:**
  1. Click Click Click ...
  1. Select one or many ...
  1. Double Click or Right Click ...

**Downloads:**
> please go to Downloads tab.

**Discussion:**
> [Simple.Net Google Group](http://groups.google.com/group/simpledotnet)

# Main Plugins #

| **Name** | **Author** | **Description** | **Link** |
|:---------|:-----------|:----------------|:---------|
| SAE.de4dot | Wicky | A simple [de4dot](https://bitbucket.org/0xd4d/de4dot) GUI |  |
| SAE.EditFile | Wicky | Call external editor to open selected assembly |  |
| SAE.FileDisassembler | Wicky | Call .Net Reflector to disassemble selected assembly, port from Denis Bauer's [Reflector.FileDisassembler](http://www.denisbauer.com/NETTools/FileDisassembler.aspx) add-in |  |
| SAE.ILMerge | Wicky | An [ILMerge](http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx) GUI |  |
| SAE.MethodSearcher | Wicky | Search .Net Reflector decompiled code |  |
| SAE.PluginSample | Wicky | A simple main plugin sample |  |
| SAE.Reflector | Wicky | Open .Net Reflector to browse selected assembly |  |

# Deobfuscator Plugins #

| **Name** | **Author** | **Description** | **Link** |
|:---------|:-----------|:----------------|:---------|
| SAE.Deobf9RayHelper | Wicky | Used to update encrypted resource name when deobfuscating assembly produced by 9Ray |  |
| SAE.DeobfPluginSample | Wicky | A simple deobfuscator plugin sample |  |
| SAE.DeobfSmartAssemblyPlugin | BeketAta | A plugin for deobfuscating .NET Reflector, obfuscated with SmartAssembly. The main goal of deobfuscation is the deobfuscated program can be running and working properly. | [Download](http://code.google.com/p/simple-assembly-explorer/issues/detail?id=154) |

# Save user settings in a SAE plugin #

**Steps:**

1. Register properties which need to be saved in plugin's constructor

Sample:
```
        public const string PropertyOutputDir = "ILMergeOutputDir";
        public const string PropertyStrongKeyFile = "ILMergeStrongKeyFile";

        public Plugin(IHost host)
        {
            host.AddProperty(PropertyOutputDir, String.Empty, typeof(String));
            host.AddProperty(PropertyStrongKeyFile, String.Empty, typeof(String));
        }
```

2. Use properties

Sample:
```
        private IHost _host;

        public string OutputDir
        {
            get { return _host.GetPropertyValue(Plugin.PropertyOutputDir) as string; }
            set { _host.SetPropertyValue(Plugin.PropertyOutputDir, value); }
        }

        public string StrongKeyFile
        {
            get { return _host.GetPropertyValue(Plugin.PropertyStrongKeyFile) as string; }
            set { _host.SetPropertyValue(Plugin.PropertyStrongKeyFile, value); }
        }
```

# Add a customized tool to SAE Tools menu #

**Steps:**

1. Open Tools.txt

2. Add a new line for your tool, the format is: `<Tool's Title>;<Executable> [Arguments]`

  * Title will be shown on Tools menu
  * Executable is the exe file
  * Arguments is parameters which you want to pass to your tool (optional)

3. Save Tools.txt

4. Open SAE and dropdown Tools menu to see your tool

**Samples:**

1. Add a link to Notepad:

Notepad;notepad.exe

2. Add a link to Outlook Express

Outlook Express;"C:\Program Files\Outlook Express\msimn.exe"

# Write a main plugin for SAE #

**Steps:**


1. Create a Class Library project, e.g. `SAE.MyPlugin`

It's recommended to keep assembly file name and name space same.

2. Add a reference to `SimpleAssemblyExplorer.Plugin.dll`.

This assembly can be found in SAE archive file.

3. Add a new class and inherit DefaultMainPlugin

SAE will firstly search all exported types for plugin, if error occured, will secondly try to use assembly file name + Plugin to search plugin (e.g. `SAE.MyPlugin.Plugin`).

And, you need to have at least a constructor with a IHost argument.

4. Override PluginInfo property and Run method

5. Build and put the assembly to SAE's Plugins directory

6. Open SAE to try the plugin

**Samples:**

You can get some SAE plugins source code from repository for reference:
  * `SAE.EditFile`
  * `SAE.FileDisassembler`
  * `SAE.ILMerge`
  * `SAE.MethodSearcher`
  * `SAE.PluginSample`
  * `SAE.Reflector`

# How to Build #

**Suggested Steps:**

1. Download and install [TortoiseSVN](http://tortoisesvn.tigris.org/)

2. Create directory structure:
```
 |-SimpleAssemblyExplorer
     |-sae.snk (generated by yourself)
```

3. Check out latest source to `SimpleAssemblyExplorer`

Latest source which require VS2010 SP1:
```
http://simple-assembly-explorer.googlecode.com/svn/trunk/ 
```

Old version which use Cecil 0.9 and built with VS2008 SP1:
```
http://simple-assembly-explorer.googlecode.com/svn/branches/1.13.3
```

Old version which use Cecil 0.6 and built with VS2008 SP1:
```
http://simple-assembly-explorer.googlecode.com/svn/branches/1.11.3
```

Finally, your directories look like:
```
 |-SimpleAssemblyExplorer
     |-sae.snk (generated by yourself)
     |-Mono.Cecil
        |-mono.snk (generated by yourself)
     |-SAE.de4dot
     |-SAE.Deobf9RayHelper
     |-SAE.DeobfPluginSample
     |-SAE.EditFile
     |-SAE.FileDisassembler
     |-SAE.ILMerge
     |-SAE.MethodSearcher
     |-SAE.PluginSample
     |-SAE.Reflector
     |-SimpleAssemblyExplorer
     |-SimpleAssemblyExplorer.Core
     |-SimpleAssemblyExplorer.Plugin
     |-SimpleProfiler
     |-SimpleUtils
     |-support
     |-TestProject
     |-TestSample
```

Please note you need to generate two strong key files (sae.snk, mono.snk) and put them in proper directory as above.

4. Open `SimpleAssemblyExplorer.sln` and build.
