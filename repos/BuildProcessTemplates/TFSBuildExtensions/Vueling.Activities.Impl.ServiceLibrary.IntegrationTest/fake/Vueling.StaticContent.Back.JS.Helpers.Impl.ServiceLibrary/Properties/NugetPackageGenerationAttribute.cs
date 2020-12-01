using System;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
namespace Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class NugetPackageGenerationAttribute : Attribute
    {
        public NugetPackageGenerationAttribute(bool description)
        {
            Description = description;
        }

        public bool Description { get; private set; }
    }
}
