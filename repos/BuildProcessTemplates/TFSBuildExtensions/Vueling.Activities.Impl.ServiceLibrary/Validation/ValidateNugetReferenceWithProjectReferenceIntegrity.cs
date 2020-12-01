using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    internal class ValidateNugetRerefenceWithProjectReferenceIntegrity
    {
        private List<string> projectNames;
        private List<List<Dependency>> packages;
        private List<List<Dependency>> csprojsfiles;
        private List<string> logErrorList;
        private XDocument libAssemblyExceptions;
        private List<FilteredLibrary> filteredLibraries;

        public ValidateNugetRerefenceWithProjectReferenceIntegrity(List<string> _projectNames, List<List<Dependency>> _packages, List<List<Dependency>> _csprojsfiles, List<string> _logErrorList,
                                                                    XDocument _libAssemblyExceptions, List<FilteredLibrary> _filteredLibraries)
        {
            projectNames = _projectNames;
            csprojsfiles = _csprojsfiles;
            logErrorList = _logErrorList;
            libAssemblyExceptions = _libAssemblyExceptions;
            packages = _packages;
            filteredLibraries = _filteredLibraries;
        }

        public bool validateNugetRerefenceWithProjectReferenceIntegrity()
        {
            bool validationOk = true;

            for (int projectIndex = 0; projectIndex < csprojsfiles.Count(); projectIndex++)
            {
                List<Dependency> projectDependencies = csprojsfiles.ElementAt(projectIndex);

                foreach (Dependency projectDependency in projectDependencies)
                {
                    if (!ValidationHelper.isLibAssemblyExceptionForThisProject(libAssemblyExceptions, projectDependency.Project, projectDependency.Name))
                    {
                        bool referenceMatchWithPackagesFile = false;

                        if (verifyFilteredAssemblies(projectDependency.Name, projectDependency.Version, projectDependency.Project))
                        {
                            List<Dependency> packageDependencies = packages.ElementAt(projectIndex);

                            foreach (Dependency packageDependency in packageDependencies)
                            {
                                if (projectDependency.Name.Equals(packageDependency.Name))
                                {
                                    if (projectDependency.Version.Equals(packageDependency.Version))
                                    {
                                        referenceMatchWithPackagesFile = true;
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            referenceMatchWithPackagesFile = true;
                            validationOk = false;
                        }

                        if (!referenceMatchWithPackagesFile)
                        {
                            logErrorList.Add("Project: " + projectDependency.Project + ". Project reference from assembly " + projectDependency.Name + " with version " + projectDependency.Version + " does not exist with packages.config.");
                            validationOk = false;
                        }
                    }
                }
            }
            return validationOk;
        }

        public bool ValidateSolutionProjectIsNotAddedAsNugetPackage()
        {
            bool validationOk = true;
            int counter = 0;

            foreach (var projectName in projectNames)
            {
                foreach (var dependencies in packages)
                {
                    foreach (var dependency in dependencies)
                    {
                        if (projectName.ToLower().Equals(dependency.Name))
                        {
                            logErrorList.Add("Project " + projectName + " from solution is also added as nuget package in project " + dependency.Project + ". Delete nuget package reference and add as project reference.");
                            validationOk = false;
                            break;
                        }
                    }
                }

                counter++;
            }

            return validationOk;
        }

        private bool verifyFilteredAssemblies(string libraryFromProject, string version, string projectValidated)
        {
            foreach (FilteredLibrary library in filteredLibraries)
            {
                if (library.library.ToLower().Equals(libraryFromProject))
                {
                    Regex regex = new Regex(library.version);
                    Match match = regex.Match(version);

                    if (match.Success)
                    {
                        if (!IsAnAllowedProject(library.allowedApplications, projectValidated))
                        {
                            logErrorList.Add("Nuget package " + library.library + " with version " + library.version + " could not be installed in application " + projectValidated);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsAnAllowedProject(List<string> allowedProjects, string projectValidated)
        {
            foreach (var project in allowedProjects)
            {
                if (project.Contains(projectValidated))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
