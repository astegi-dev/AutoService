using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(AutoService.AssemblyInfo.Name)]
[assembly: AssemblyDescription(AutoService.AssemblyInfo.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct(AutoService.AssemblyInfo.Product)]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion(AutoService.AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AutoService.AssemblyInfo.Version)]

namespace AutoService
{
    public abstract class AssemblyInfo
    {
        public const string Name = "AutoService";
        public const string Product = "Auto Service";
        public const string Description = "Automatic service start / stop system for Visual Studio";
        public const string Version = "1.0.0.0";
    }
}