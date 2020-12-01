using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion;
using Vueling.Activities.Contracts.ServiceLibrary.Validation;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    [RegisterServiceAttribute]
    public class AssemblyVersionValidationService : IAssemblyVersionValidationService
    {
        private List<string> projectNames;
        private List<List<Dependency>> packages;
        private List<List<Dependency>> csprojsfiles;

        private List<string> projectList;

        private string sourcesDirectory;

        private string environment;

        private IEnumerable<string> assemblyExceptions;

        private IEnumerable<string> assembliesToProcess;

        private string fullPathLibAssemblyExceptions;

        private XDocument libAssemblyExceptions;
        private List<FilteredLibrary> filteredLibraries;

        public bool AreVersionValid { get; set; }

        private List<string> logErrorList = new List<string>();
        private List<string> logInformationList = new List<string>();
        private List<string> logWarningList = new List<string>();

        public List<string> LogErrorList
        {
            get { return logErrorList; }
        }

        public List<string> LogWarningList
        {
            get { return logWarningList; }
        }

        public List<string> LogInformationList
        {
            get { return logInformationList; }
        }

        public void Initialize(List<string> _projectList,
                                                string _sourcesDirectory,
                                                string _environment,
                                                IEnumerable<string> _assemblyExceptions,
                                                IEnumerable<string> _assembliesToProcess,
                                                string _fullPathLibAssemblyExceptions,
                                                string _fullPathfilteredLibraries)
        {
            projectList = _projectList;
            sourcesDirectory = _sourcesDirectory;
            environment = _environment;
            assemblyExceptions = _assemblyExceptions;
            assembliesToProcess = _assembliesToProcess;
            fullPathLibAssemblyExceptions = _fullPathLibAssemblyExceptions;

            libAssemblyExceptions = XDocument.Load(fullPathLibAssemblyExceptions);

            using (StreamReader read = new StreamReader(_fullPathfilteredLibraries))
            {
                string json = read.ReadToEnd();
                filteredLibraries = JsonConvert.DeserializeObject<List<FilteredLibrary>>(json);
            }
        }

        public void InternalExecute()
        {
            try
            {
                projectNames = new List<string>();
                packages = new List<List<Dependency>>();
                csprojsfiles = new List<List<Dependency>>();

                Initialize();
                ProcessAssembliesVersion();
            }
            finally
            {
                packages = null;
                csprojsfiles = null;
            }
        }

        private void Initialize()
        {
            if (assembliesToProcess == null || assembliesToProcess.Count() == 0)
            {
                assembliesToProcess = new List<string> { "*" };
            }

            if (assemblyExceptions == null)
            {
                assemblyExceptions = new List<string> { "NONE" };
            }
        }

        private void ProcessAssembliesVersion()
        {
            this.AreVersionValid = true;

            foreach (string project in projectList)
            {
                if (!ValidationHelper.IsException(assemblyExceptions, project))
                {
                    ValidateAssemblyFile validateAssemblyFile = new ValidateAssemblyFile(sourcesDirectory, environment, assemblyExceptions, assembliesToProcess, logErrorList, logWarningList);


                    if (!validateAssemblyFile.verifyVersionAssemblies(project))
                    {
                        this.AreVersionValid = false;
                    }
                    if (!validateAssemblyFile.verifyDescriptionAssemblies(project))
                    {
                        this.AreVersionValid = false;
                    }
                }

                projectNames.Add(project);

                if (!loadProjectReferencesToList(project))
                {
                    logErrorList.Add("Error loading references from project " + project + ".");
                    this.AreVersionValid = false;
                }
                loadPackagesFileToList(project);
            }

            if (this.AreVersionValid)
            {
                logInformationList.Add("Assembly version number validated successfully.");
            }

            ValidateNugetReferenceIntegrity validateNugetReferenceIntegrity = new ValidateNugetReferenceIntegrity(packages, logErrorList);

            if (!validateNugetReferenceIntegrity.validateNugetReferenceIntegrity())
            {
                this.AreVersionValid = false;
            }
            else
            {
                logInformationList.Add("All nuget reference from package.config files of projects' solution contains the same version of unique packages.");
            }

            ValidateNugetRerefenceWithProjectReferenceIntegrity validateNugetRerefenceWithProjectReferenceIntegrity = new ValidateNugetRerefenceWithProjectReferenceIntegrity(projectNames, packages, csprojsfiles, logErrorList, libAssemblyExceptions, filteredLibraries);

            if (!validateNugetRerefenceWithProjectReferenceIntegrity.validateNugetRerefenceWithProjectReferenceIntegrity())
            {
                this.AreVersionValid = false;
            }
            else
            {
                logInformationList.Add("All nuget reference are consistent with project references.");
            }

            if (!validateNugetRerefenceWithProjectReferenceIntegrity.ValidateSolutionProjectIsNotAddedAsNugetPackage())
            {
                this.AreVersionValid = false;
            }

            if (!this.AreVersionValid)
            {
                logErrorList.Add("Error validating solution.");
            }
        }

        private bool loadProjectReferencesToList(string project)
        {
            bool assembliesOk = true;
            string sReferenceInclude, sHintPath;
            Dependency dependency;

            List<Dependency> references = new List<Dependency>();
            string csprojPath = sourcesDirectory + @"\" + project + @"\" + project + ".csproj";

            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            if (File.Exists(csprojPath))
            {
                XDocument projDefinition = XDocument.Load(csprojPath);

                IEnumerable<XElement> xmlReferences = projDefinition
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup")
                    .Elements(msbuild + "Reference")
                    .Where(r => r.Attribute("Include") != null);

                foreach (XElement reference in xmlReferences)
                {
                    if (reference.Element(msbuild + "HintPath") != null)
                    {
                        sReferenceInclude = reference.Attribute("Include").ToString();

                        if (reference.Element(msbuild + "HintPath").ToString().Contains(@"..\packages\"))
                        {
                            dependency = new Dependency();
                            dependency.Project = project;

                            sHintPath = reference.Element(msbuild + "HintPath").Value;

                            dependency.Name = getNameFromHintPath(sHintPath).ToLower();
                            dependency.Version = sHintPath.Substring(@"..\packages\".Length + dependency.Name.Length + 1, sHintPath.ToLower().IndexOf(@"lib\") - dependency.Name.Length - 14);

                            //In case assembly version has only 3 digits, add forth digit (forced compatibility with old nuget )
                            if (dependency.Version.Split('.').Count() < 4)
                            {
                                dependency.Version = dependency.Version + ".0";
                            }

                            references.Add(dependency);
                        }
                        else
                        {
                            if (!ValidationHelper.isLibAssemblyExceptionForThisProject(libAssemblyExceptions, project, (sReferenceInclude.Contains("Version=") ? sReferenceInclude.Substring(9, sReferenceInclude.IndexOf(", ") - 9) : sReferenceInclude.Substring(9, sReferenceInclude.Length - 10))))
                            {
                                logErrorList.Add("Referenced assembly " + sReferenceInclude + " in project " + project + " must be referenced from nuget repository, or added to lib exceptions.");
                                assembliesOk = false;
                            }
                        }
                    }
                }
            }
            csprojsfiles.Add(references);

            return assembliesOk;
        }

        private string getNameFromHintPath(string sHintPath)
        {
            string sAssemblyNameAndVersion = sHintPath.Substring(@"..\packages\".Length, sHintPath.ToLower().IndexOf(@"\lib\") - @"..\packages\".Length);
            StringBuilder name = new StringBuilder();

            for (int count = 0; count < sAssemblyNameAndVersion.Split('.').Count(); count++)
            {
                if (count > 0 && Char.IsLetter(sAssemblyNameAndVersion.Split('.')[count].ToCharArray()[0]))
                {
                    name.Append(".");
                }
                if (Char.IsLetter(sAssemblyNameAndVersion.Split('.')[count].ToCharArray()[0]))
                {
                    name.Append(sAssemblyNameAndVersion.Split('.')[count]);
                }
                else
                {
                    break;
                }
            }

            return name.ToString();
        }

        private void loadPackagesFileToList(string project)
        {
            Dependency dependency;
            List<Dependency> packagesList = new List<Dependency>();
            string packagesPath = sourcesDirectory + @"\" + project + @"\packages.config";

            if (File.Exists(packagesPath))
            {
                XDocument packageXmlFile = XDocument.Load(packagesPath);

                IEnumerable<XElement> packageList = packageXmlFile.Element("packages").Elements();

                foreach (XElement package in packageList)
                {
                    dependency = new Dependency();
                    dependency.Project = project;
                    dependency.Name = package.Attribute("id").Value.ToLower();
                    dependency.Version = package.Attribute("version").Value;
                    //In case assembly version has only 3 digits, add forth digit (forced compatibility with old nuget )
                    if (dependency.Version.Split('.').Count() < 4)
                    {
                        dependency.Version = dependency.Version + ".0";
                    }

                    packagesList.Add(dependency);
                }
            }

            packages.Add(packagesList);
        }
    }
}