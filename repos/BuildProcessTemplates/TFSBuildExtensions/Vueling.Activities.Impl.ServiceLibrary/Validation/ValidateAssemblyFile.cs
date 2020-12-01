using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vueling.Activities.Impl.ServiceLibrary.Validation
{
    internal class ValidateAssemblyFile
    {
        private string sourcesDirectory;
        private string environment;
        private IEnumerable<string> assemblyExceptions;
        private IEnumerable<string> assembliesToProcess;
        private List<string> logErrorList = new List<string>();
        private List<string> logWarningList = new List<string>();

        public ValidateAssemblyFile(string _sourcesDirectory, string _enviornment, IEnumerable<string> _assemblyExceptions,
                                                IEnumerable<string> _assembliesToProcess, List<string> _logErrorList, List<string> _logWarningList)
        {
            sourcesDirectory = _sourcesDirectory;
            environment = _enviornment;
            assemblyExceptions = _assemblyExceptions;
            assembliesToProcess = _assembliesToProcess;
            logErrorList = _logErrorList;
            logWarningList = _logWarningList;
        }

        public bool verifyDescriptionAssemblies(string project)
        {
            bool ret = true;
            Regex AssemblyInfoDescriptionExpression = new Regex("\\[assembly: AssemblyDescription\\(\"(?<description>[ |&-`´'_.,;:¿!¿?A-Za-zÀÈÌÒÙÜàèìÏòùáéíïóúÜAÉÍÓÚÑñ00-9 ]*)\"\\)\\]");
            string fileAssemblyInfoPath = sourcesDirectory + @"\" + project + @"\Properties\AssemblyInfo.cs";

            if (project.EndsWith("ServiceLibrary") || project.EndsWith("Library") || project.EndsWith("Message") || project.EndsWith("DTO") || project.EndsWith("Infrastructure"))
            {
                if (File.Exists(fileAssemblyInfoPath))
                {
                    string assemblyInfoCode = File.ReadAllText(fileAssemblyInfoPath);
                    Match match = AssemblyInfoDescriptionExpression.Match(assemblyInfoCode);
                    // When found
                    if (match.Success)
                    {
                        string str = AssemblyInfoDescriptionExpression.Match(assemblyInfoCode).Groups[AssemblyInfoDescriptionExpression.GroupNumberFromName("description")].Value;
                        if (String.IsNullOrEmpty(str))
                        {
                            logErrorList.Add("Description field of project " + project + " must contains information about new release. Description field cannot be null or empty.");
                            ret = false;
                        }
                    }
                }
                else
                {
                    logWarningList.Add(string.Format("AssemblyInfo.cs file does not exist in project {0}", project));
                }
            }

            return ret;
        }

        public bool verifyVersionAssemblies(string project)
        {
            bool ret = true;
            string attribute = "AssemblyVersion";
            // Define the regular expression to find (which is for example 'AssemblyFileVersion("1.0.0.0")' )
            Regex regex = new Regex(attribute + @"\(""\d+\.\d+\.\d+\.\d+""\)");

            // Read the text from the AssemblyInfo file
            string fileAssemblyInfoPath = sourcesDirectory + @"\" + project + @"\Properties\AssemblyInfo.cs";

            if (File.Exists(fileAssemblyInfoPath))
            {
                string assemblyInfoCode = File.ReadAllText(fileAssemblyInfoPath);
                // Search for the first occurrence of the version attribute
                Match match = regex.Match(assemblyInfoCode);
                // When found
                if (match.Success)
                {
                    string assemblyTitle = "";
                    //Get Assembly title
                    string assemblyTitleAttribute = "AssemblyTitle";
                    Regex regexAssemblyTitle = new Regex(assemblyTitleAttribute + @"\(""[A-Za-z0-9.]*""\)");
                    Match matchAssemblyTitle = regexAssemblyTitle.Match(assemblyInfoCode);
                    try
                    {
                        assemblyTitle = matchAssemblyTitle.Value.Substring(assemblyTitleAttribute.Length + 2, matchAssemblyTitle.Value.Length - assemblyTitleAttribute.Length - 4);
                    }
                    catch (Exception)
                    {
                        logErrorList.Add("Assembly title from project " + project + " could not be matched. Please add AssemblyTitle name without spaces to assembly info file.");

                        ret = false;
                    }

                    if (environment.Equals("PRE") && match.Value.EndsWith("-INT") && (assemblyTitle.EndsWith("ServiceLibrary") || assemblyTitle.EndsWith("Infrastructure") || assemblyTitle.EndsWith("Library") || assemblyTitle.EndsWith("Message") || assemblyTitle.EndsWith("DTO")))
                    {
                        logErrorList.Add(string.Format("Version revision from assembly {0} must be a stable version (now it's {1})." +
                                                    "\n Nuget packages used in PRE environment must be stable versions.", project, match.Value.Substring(attribute.Length + 2, match.Value.Length - attribute.Length - 4)));
                        ret = false;
                    }
                }
            }
            else
            {
                logWarningList.Add(string.Format("AssemblyInfo.cs file does not exist in project {0}", project));
            }

            VerifyVersionPackageAssemblies verifyVersionPackageAssemblies = new VerifyVersionPackageAssemblies(sourcesDirectory, logErrorList, logWarningList, assembliesToProcess, assemblyExceptions);

            return (ret && verifyVersionPackageAssemblies.verifyVersionPackageAssemblies(project, environment));
        }
    }
}
