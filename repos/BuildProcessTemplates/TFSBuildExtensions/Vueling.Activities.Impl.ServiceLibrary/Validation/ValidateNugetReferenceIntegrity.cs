using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    internal class ValidateNugetReferenceIntegrity
    {
        private List<List<Dependency>> packages;
        private List<string> logErrorList;

        public ValidateNugetReferenceIntegrity(List<List<Dependency>> _packages, List<string> _logErrorList)
        {
            packages = _packages;
            logErrorList = _logErrorList;
        }

        public bool validateNugetReferenceIntegrity()
        {
            bool assembliesOk = true;
            List<Dependency> assemblyFirstOccurrence = new List<Dependency>();

            foreach (List<Dependency> references in packages)
            {
                foreach (Dependency reference in references)
                {
                    bool registeredAssemblyFirstOccurrence = false;

                    foreach (Dependency occurrence in assemblyFirstOccurrence)
                    {
                        if (occurrence.Name.Equals(reference.Name))
                        {
                            if (!occurrence.Version.Equals(reference.Version))
                            {
                                logErrorList.Add("Project: " + occurrence.Project + ". Reference version assembly from " + occurrence.Name + " with version " + occurrence.Version + " does not match with packages.config from project " + reference.Project + " (version in  packages config for this assembly is " + reference.Version + ").");
                                assembliesOk = false;
                            }

                            registeredAssemblyFirstOccurrence = true;
                        }
                    }

                    if (!registeredAssemblyFirstOccurrence)
                    {
                        assemblyFirstOccurrence.Add(reference);
                    }
                }
            }
            return assembliesOk;
        }
    }
}