using System;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
namespace Vueling.XXX.Impl.ServiceLibrary.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class NugetPackageGenerationAttribute : Attribute
    {
        private bool _nugetPackageGeneration;

        public NugetPackageGenerationAttribute(bool description)
        {
            _nugetPackageGeneration = description;
        }

        public bool Description 
        { 
            get { return _nugetPackageGeneration; }
        }
    }
}
