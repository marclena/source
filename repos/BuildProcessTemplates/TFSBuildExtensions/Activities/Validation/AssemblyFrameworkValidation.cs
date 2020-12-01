using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Activities;
using System.IO;

namespace TFSBuildExtensions.EnvironmentManagement.Validation
{
    public sealed class AssemblyFrameworkValidation : BaseCodeActivity
    {
        [Description("Path where analyze assemblies.")]
        public InArgument<string> Path { get; set; }

        [Description("Framework version to match with assemblies")]
        public InArgument<string> AssemblyFrameworkVersion { get; set; }

        [Description("All framework version matches true or false")]
        public OutArgument<bool> Result { get; set; }

        protected override void InternalExecute()
        {
            string assemblyFrameworkVersion;
            List<string> assemblies = System.IO.Directory.GetFiles(this.Path.Get(this.ActivityContext), "*.dll").ToList();

            this.Result.Set(this.ActivityContext, true);

            foreach (string assembly in assemblies)
            {
                System.Reflection.Assembly assemblyFile = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(assembly));
                assemblyFrameworkVersion = assemblyFile.ImageRuntimeVersion;

                if (this.AssemblyFrameworkVersion.Get(this.ActivityContext).CompareTo(assemblyFrameworkVersion) < 0)
                {
                    this.Result.Set(this.ActivityContext, false);
                    LogBuildWarning(String.Format("Assembly {0} is compiled with framework {1}. You only can use assemblies compiled til framework {2}.", assemblyFile.GetName(), assemblyFrameworkVersion, this.AssemblyFrameworkVersion.Get(this.ActivityContext)));
                }

                
            }
        }
    }
}
