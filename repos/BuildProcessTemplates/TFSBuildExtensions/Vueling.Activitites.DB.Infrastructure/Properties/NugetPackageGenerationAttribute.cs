using System;
using System.Runtime.InteropServices;

namespace Vueling.Activitites.DB.Infrastructure.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public class NugetPackageGenerationAttribute : Attribute
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
