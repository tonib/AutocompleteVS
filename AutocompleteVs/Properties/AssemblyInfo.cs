using Microsoft.VisualStudio.Shell;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("AutocompleteVs")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutocompleteVs")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.13.0.0")]
[assembly: AssemblyFileVersion("0.13.0.0")]

// Required for VS 2019: VS by default loads an older version of the BCL.AsyncInterfaces assembly
// and Ollamasharp / OpenAI / etc needs version 9
[assembly: ProvideBindingRedirection(AssemblyName = "Microsoft.Bcl.AsyncInterfaces",
    NewVersion = "9.0.0.0", OldVersionLowerBound = "1.0.0.0",
    OldVersionUpperBound = "8.0.0.0")]