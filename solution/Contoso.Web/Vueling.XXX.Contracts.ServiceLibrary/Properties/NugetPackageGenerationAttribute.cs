using System;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
namespace Vueling.XXX.Contracts.ServiceLibrary.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class NugetPackageGenerationAttribute : Attribute
    {
        private bool _nugetPackageGeneration;

        public NugetPackageGenerationAttribute(bool Description)
        {
            _nugetPackageGeneration = Description;
        }

        public bool Description
        {
            get { return _nugetPackageGeneration; }
        }
    }
}
