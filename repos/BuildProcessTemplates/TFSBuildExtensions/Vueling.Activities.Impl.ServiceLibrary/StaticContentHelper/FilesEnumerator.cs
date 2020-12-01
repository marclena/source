using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Impl.ServiceLibrary.StaticContentHelper
{
    public class FilesEnumerator
    {
        private static bool FileEndsWithOneValidExtension(string fileName, HashSet<string> filterByFileExtension)
        {
            if (!filterByFileExtension.Any())
                return true;
            int lastPointPosition = fileName.LastIndexOf('.');
            if (lastPointPosition >= 0)
            {
                lastPointPosition++;
                string extension = fileName.Substring(lastPointPosition, fileName.Length - lastPointPosition).ToLower();
                return filterByFileExtension.Contains(extension);
            }
            return false;
        }

        private static List<string> GetAllFilesRecursively(string path, HashSet<string> filterByFileExtension)
        {
            var directories = Directory.EnumerateDirectories(path);
            List<string> filesCurrentFolder = new List<string>();
            foreach (var dir in directories)
                filesCurrentFolder.AddRange(GetAllFilesRecursively(dir, filterByFileExtension));
            filesCurrentFolder.AddRange(Directory.EnumerateFiles(path).Where(c => FileEndsWithOneValidExtension(c, filterByFileExtension)));
            return filesCurrentFolder;
        }

        private static string TrunkRelPath(string file, string baseFolder)
        {
            int iStart = file.IndexOf(baseFolder) + baseFolder.Length;
            file = file.Substring(iStart);
            return file;
        }

        private static List<string> TrunkRelPathInFiles(List<string> filesList, string baseFolder)
        {
            List<string> truncatedList = new List<string>();
            foreach (var file in filesList)
            {
                truncatedList.Add(TrunkRelPath(file, baseFolder));
            }
            return truncatedList;
        }

        public static List<string> GetFilesToProcess(string path, HashSet<string> filterByFileExtension)
        {
            var filesListed = GetAllFilesRecursively(path, filterByFileExtension);
            return TrunkRelPathInFiles(filesListed, path);
        }

        public static Dictionary<string, HashSet<string>> CalculateFoldersInFolders(List<string> folders)
        {
            Dictionary<string, HashSet<string>> calculatedFolders = new Dictionary<string, HashSet<string>>();
            foreach (var folder in folders)
            {
                var sFolder = folder;
                //var splittedFolder = folder.Split('\\');
                string parentFolder = string.Empty;
                string currentFolder = sFolder;

                if (sFolder.LastIndexOf('\\') > 0)
                {
                    parentFolder = sFolder.Substring(0, folder.LastIndexOf('\\'));
                    currentFolder = sFolder.Substring(folder.LastIndexOf('\\'));
                }
                if (!calculatedFolders.ContainsKey(parentFolder))
                    calculatedFolders.Add(parentFolder, new HashSet<string>());
                if (!calculatedFolders[parentFolder].Contains(currentFolder))
                    calculatedFolders[parentFolder].Add(currentFolder);

            }
            return calculatedFolders;
        }

        public static List<string> GetAllFolders(List<string> paths)
        {
            HashSet<string> folders = new HashSet<string>();
            foreach (var path in paths)
            {
                var splittedPath = path.Split('\\');
                for (int i = 0; i < splittedPath.Length; i++)
                {
                    var currentPath = "";
                    for (int j = 0; j <= i; j++)
                    {
                        currentPath += '\\' + splittedPath[j];
                    }
                    if (currentPath.StartsWith("\\\\"))
                        currentPath = currentPath.Substring(1);
                    if (!folders.Contains(currentPath) && !string.IsNullOrEmpty(currentPath) && (currentPath != "\\"))
                        folders.Add(currentPath);
                }
            }
            return folders.ToList();
        }
    }

}
