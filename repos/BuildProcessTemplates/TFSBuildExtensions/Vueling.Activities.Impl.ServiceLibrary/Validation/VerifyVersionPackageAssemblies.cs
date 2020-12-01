using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    internal class VerifyVersionPackageAssemblies
    {
        private string sourcesDirectory;
        private List<string> logErrorList;
        private List<string> logWarningList;
        private IEnumerable<string> assembliesToProcess;
        private IEnumerable<string> assemblyExceptions;

        public VerifyVersionPackageAssemblies(string _sourcesDirectory, List<string> _logErrorList, List<string> _logWarningList, IEnumerable<string> _assembliesToProcess, IEnumerable<string> _assemblyExceptions)
        {
            sourcesDirectory = _sourcesDirectory;
            logErrorList = _logErrorList;
            logWarningList = _logWarningList;
            assembliesToProcess = _assembliesToProcess;
            assemblyExceptions = _assemblyExceptions;
        }

        public bool verifyVersionPackageAssemblies(string project, string environment)
        {
            bool ret = true;
            string filePackagesConfigPath = sourcesDirectory + @"\" + project + @"\packages.config";

            if (environment.Equals("PRE"))
            {
                if (File.Exists(filePackagesConfigPath))
                {
                    string xmlPackagesConfig = File.ReadAllText(filePackagesConfigPath);

                    var xPackagesConfig = XDocument.Load(XmlReader.Create(new StringReader(xmlPackagesConfig)));

                    var packages = xPackagesConfig.Element("packages").Elements();

                    foreach (var package in packages)
                    {
                        Dependency dependency = new Dependency();
                        dependency.Name = package.Attribute("id").Value;
                        dependency.Version = package.Attribute("version").Value;

                        if (!ValidationHelper.IsException(assemblyExceptions, dependency.Name) && IsPatternCompliance(dependency.Name) &&
                            dependency.Version.EndsWith("-INT"))
                        {
                            logErrorList.Add(
                                string.Format(
                                    "Dependency error in project {0}. Version revision from assembly {1} must be a stable version (now it's {2})." +
                                                    "\n Nuget packages used in PRE environment must be stable versions.",
                                    project, dependency.Name, dependency.Version));
                            ret = false;
                        }
                    }
                }
                else
                {
                    logWarningList.Add(string.Format("Files packages.config does not exists in project {0}", project));
                }
            }

            return ret;
        }

        private bool IsPatternCompliance(string project)
        {
            foreach (var pattern in assembliesToProcess)
            {
                if (pattern.ToString().Equals("*"))
                {
                    return true;
                }
                if (project.StartsWith(pattern.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}