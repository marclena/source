﻿using System;
using System.Runtime.InteropServices;


namespace Vueling.XXX.DB.Infrastructure.Properties
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
