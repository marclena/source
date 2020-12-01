using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.StaticContentHelper;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.StaticContentHelper
{
    [RegisterServiceAttribute]
    public class StaticContentHelperGeneratorService : BaseActivityService, IStaticContentHelperGenerator
    {
        private readonly HashSet<string> StaticContentHelperFilterExtensions = new HashSet<string>() { "css", "js", "ttf" };
        private readonly HashSet<string> StaticContentHelperInvalidClassNameCharacters = new HashSet<string>() { ".", "-" };
        private const string StaticContentHelperDefaultSpaceString = "_";

        private string _sourceDirectory;
        private string _targetDirectory;
        private string _staticContentWebUINamespace;

        public void Initialize(string sourceDirectory, string targetDirectory, string staticContentWebUINamespace)
        {
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
            _staticContentWebUINamespace = staticContentWebUINamespace;
        }

        private static Project targetCsProject { get; set; }
        private static void CreateFoldersIfDontExists(string folder)
        {
            Directory.CreateDirectory(folder);
        }

        private static void AddFileToCsproj(string filepath)
        {
            if (filepath.StartsWith("\\"))
                filepath = filepath.Substring(1);
            targetCsProject.AddItem("Compile", filepath, null);
        }

        private static void LoadTargetCsProject(string _targetDirectory)
        {
            var collection = new ProjectCollection();
            int lastIndexOfBackslash = _targetDirectory.LastIndexOf("\\") + 1;
            string csprojFile = _targetDirectory.Substring(lastIndexOfBackslash,
                _targetDirectory.Length - lastIndexOfBackslash) + ".csproj";
            targetCsProject =
                collection.LoadProject(
                    _targetDirectory + "\\" + csprojFile);
            foreach (var projItem in targetCsProject.Items.Where(c => c.ItemType == "Compile" && c.EvaluatedInclude.Contains("Helpers")).ToList())
            {
                targetCsProject.RemoveItem(projItem);
            }
        }

        private static void SaveTargetCsProject()
        {
            targetCsProject.Save();
        }

        public override void InternalExecute()
        {
            if (!Directory.Exists(_targetDirectory))
                return;

            LoadTargetCsProject(_targetDirectory);

            DeleteOldHelpersFolder();

            GenerateHelpersFolder();

            SaveTargetCsProject();
        }

        private void DeleteOldHelpersFolder()
        {
            if (Directory.Exists(_targetDirectory + "\\Helpers"))
                Directory.Delete(_targetDirectory + "\\Helpers", true);
        }

        private string GetStaticType()
        {
            int lastIndexOfDot = _sourceDirectory.LastIndexOf(".WebUI");
            string staticType = "";
            if (lastIndexOfDot >= 0)
            {
                staticType = _sourceDirectory.Substring(0, lastIndexOfDot);
            }
            lastIndexOfDot = staticType.LastIndexOf(".");
            if (lastIndexOfDot >= 0)
            {
                staticType = staticType.Substring(lastIndexOfDot + 1);
            }
            return staticType;
        }

        private void GenerateHelpersFolder()
        {
            var filesListed = FilesEnumerator.GetFilesToProcess(_sourceDirectory, StaticContentHelperFilterExtensions);
            var foldersWithFiles = filesListed.GroupBy(c => c.Substring(0, c.LastIndexOf("\\"))).ToDictionary(c => c.Key, c => c.ToList());
            var folders = FilesEnumerator.GetAllFolders(foldersWithFiles.Keys.ToList());
            var foldersInFolder = FilesEnumerator.CalculateFoldersInFolders(folders);
            folders = folders.OrderByDescending(c => c.Count(cc => cc == '\\')).ToList();
            List<string> baseClasses = new List<string>();
            foreach (var folder in folders)
            {
                var folderBase = "Helpers" + folder.Substring(0, folder.LastIndexOf("\\"));
                var fileName = folder.Substring(folder.LastIndexOf("\\") + 1);
                var formattedFileName = char.ToUpper(fileName[0]) + fileName.Substring(1);
                if (formattedFileName == "Vueling")
                    formattedFileName = "VuelingFolder";

                Trace.TraceInformation("Processing " + folder);
                var foldersIncluded = foldersInFolder.ContainsKey(folder)
                    ? foldersInFolder[folder].ToList()
                    : new List<string>();
                var filesIncluded = foldersWithFiles.ContainsKey(folder)
                    ? foldersWithFiles[folder].ToList()
                    : new List<string>();
                CodeGenerator sample = new CodeGenerator(folder, filesIncluded, foldersIncluded, StaticContentHelperInvalidClassNameCharacters, StaticContentHelperDefaultSpaceString, _staticContentWebUINamespace);
                sample.AddFields();

                if (folderBase == "Helpers")
                    baseClasses.Add(formattedFileName);
                CreateFoldersIfDontExists(_targetDirectory + "\\" + folderBase);
                sample.GenerateCSharpCode(_targetDirectory + "\\" + folderBase + "\\" + formattedFileName + ".cs");
                AddFileToCsproj(folderBase + "\\" + formattedFileName + ".cs");
            }
            string staticResources = "Static" + GetStaticType();
            string staticResourcesFileName = "\\Helpers\\" + staticResources + ".cs";
            CodeGenerator staticResourcesGenerator = new CodeGenerator("\\", new List<string>(), baseClasses, StaticContentHelperInvalidClassNameCharacters, StaticContentHelperDefaultSpaceString, _staticContentWebUINamespace, staticResources);
            staticResourcesGenerator.AddBaseFileFields();
            staticResourcesGenerator.GenerateCSharpCode(_targetDirectory + staticResourcesFileName);
            AddFileToCsproj("Helpers\\" + staticResources + ".cs");
        }
    }
}