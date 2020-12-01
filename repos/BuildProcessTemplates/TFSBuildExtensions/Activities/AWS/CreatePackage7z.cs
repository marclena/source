using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.AWS
{
    public class CreatePackage7z
    {
        public void CreatePackageFile(List<string> projectsToInclude, string binariesDirectory)
        {
            Parallel.ForEach(projectsToInclude, application =>
            {
                CopyApplicationToPathAWS(application, binariesDirectory);
            });
        }

        internal void CopyApplicationToPathAWS(string application, string binariesDirectory)
        {
            string fileExtensionsExcluded = ".dacpac,.error,.log,.CodeAnalysisLog.xml,.lastcodeanalysissucceeded,.error";
            string directoriesExcluded = "logs,_PublishedWebsites,AWS";

            if (application.ToLower().StartsWith("skysales"))
            {
                DirectoryCopy(binariesDirectory + @"\_PublishedWebsites\" + application, binariesDirectory + @"\AWS\SkySales", true, fileExtensionsExcluded, directoriesExcluded);
            }
            else if (application.ToLower().Contains("staticcontent"))
            {
                DirectoryCopy(binariesDirectory + @"\_PublishedWebsites\" + application, binariesDirectory + @"\AWS\staticcontent", true, fileExtensionsExcluded, directoriesExcluded);
            }
            else
            {
                switch (application.Substring(application.LastIndexOf(".")).ToLower())
                {
                    case ".config":
                        DirectoryCopy(binariesDirectory + @"\..\Sources\" + application, binariesDirectory + @"\AWS\" + application, true, fileExtensionsExcluded, directoriesExcluded);
                        break;
                    case ".windowsservice":
                        DirectoryCopy(binariesDirectory, binariesDirectory + @"\AWS\" + application, true, fileExtensionsExcluded, directoriesExcluded);
                        break;
                    case ".webservice":
                        DirectoryCopy(binariesDirectory + @"\_PublishedWebsites\" + application, binariesDirectory + @"\AWS\" + application, true, fileExtensionsExcluded, directoriesExcluded);
                        break;
                    case ".webui":
                        DirectoryCopy(binariesDirectory + @"\_PublishedWebsites\" + application, binariesDirectory + @"\AWS\" + application, true, fileExtensionsExcluded, directoriesExcluded);
                        break;
                    case ".webapi":
                        DirectoryCopy(binariesDirectory + @"\_PublishedWebsites\" + application, binariesDirectory + @"\AWS\" + application, true, fileExtensionsExcluded, directoriesExcluded);
                        break;
                    case ".database":
                        Directory.CreateDirectory(binariesDirectory + @"\AWS\" + application);
                        FileInfo file = new FileInfo(binariesDirectory + @"\" + application + ".dacpac");
                        file.CopyTo(binariesDirectory + @"\AWS\" + application + @"\" + application + ".dacpac");
                        break;
                }
            }
        }

        internal void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, string fileExtensionsExcluded, string directoriesExcluded)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (IsItemAllowedToBeCopied(fileExtensionsExcluded, file.Name))
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, true);
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    if (IsItemAllowedToBeCopied(directoriesExcluded, temppath))
                    {
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, fileExtensionsExcluded, directoriesExcluded);
                    }
                }
            }
        }

        internal bool IsItemAllowedToBeCopied(string itemsExcluded, string items)
        {
            foreach (var exclusion in itemsExcluded.Split(',').ToList())
            {
                if (items.EndsWith(exclusion))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
