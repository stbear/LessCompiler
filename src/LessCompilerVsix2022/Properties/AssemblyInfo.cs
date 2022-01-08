using System.Reflection;
using System.Runtime.InteropServices;
using LessCompilerVsix;

[assembly: AssemblyTitle(Constants.VSIX_NAME)]
[assembly: AssemblyDescription("Compiles LESS files directly within Visual Studio")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Stanislav Bakhuta")]
[assembly: AssemblyProduct(Constants.VSIX_NAME)]
[assembly: AssemblyCopyright("Stanislav Bakhuta")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("en-US")]
[assembly: ComVisible(false)]

[assembly: AssemblyVersion(LessCompilerPackage.Version)]
[assembly: AssemblyFileVersion(LessCompilerPackage.Version)]
