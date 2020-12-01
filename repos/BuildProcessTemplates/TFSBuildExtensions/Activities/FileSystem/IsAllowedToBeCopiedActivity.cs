using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.IO;

namespace TFSBuildExtensions.FileSystem
{
    /// <summary>
    /// Determine whether a file is allowed to be copied dealing with exclude filter and filter exclusions
    /// </summary>
    [BuildActivity(HostEnvironmentOption.All)]
    public class IsAllowedToBeCopiedActivity : BaseCodeActivity
    {
        [Description("File path file to evaluate")]
        [RequiredArgument]
        public InArgument<string> File { get; set; }

        [Description("Website root directory that you will capture web content from")]
        [RequiredArgument]
        public InArgument<string> WebsiteRoot { get; set; }

        [Description("Exclude Filter represents files and or directories to be excluded from the snapshot by default.  These typically include Web.config (ASP.NET), and Binary file content in bin/*.")]
        [RequiredArgument]
        public InArgument<string> ExcludeFilter { get; set; }

        [Description("Filter Exclusions is used to override the Exclude Filter by specifying specific exceptions that MUST be included.  Common examples include custom DLL files in the bin/ folder.  Custom DLL’s must be added to this exclusions list.")]
        [RequiredArgument]
        public InArgument<string> FilterExclusions { get; set; }

        [Description("true: file is allowed to be copied. false: file is not allowed to be copied")]
        public OutArgument<bool> IsAllowed { get; set; }

        protected override void InternalExecute()
        {
            FileFilter();
        }

        private void FileFilter()
        {
            this.ActivityContext.SetValue(IsAllowed, false);
            WildcardFileFilter wildcardFileFilter = new WildcardFileFilter(this.ActivityContext.GetValue(FilterExclusions), this.ActivityContext.GetValue(ExcludeFilter));

            if (wildcardFileFilter.IncludeFile(this.ActivityContext.GetValue(File)))
            {
                this.ActivityContext.SetValue(IsAllowed, true);
            }
        }
    }

    [Serializable]
    public class WildcardFileFilter
    {
        private List<string> _includeList;
        private List<string> _excludeList;
        public WildcardFileFilter(string includeList, string excludeList)
        {
            this._includeList = new List<string>();
            this._excludeList = new List<string>();
            if (includeList == null)
            {
                this._includeList.Add("*");
            }
            else
            {
                includeList = includeList.ToLower();
                if (includeList.Length == 0)
                {
                    this._includeList.Add("*");
                }
                else
                {
                    string[] includeItems = includeList.Split(new char[]
					{
						','
					});
                    string[] array = includeItems;
                    for (int j = 0; j < array.Length; j++)
                    {
                        string includeItem = array[j];
                        string i = includeItem.Trim();
                        if (i.EndsWith("\\"))
                        {
                            throw new ArgumentException("Include list criteria \"" + includeItem + "\" can not be a directory (end in \\)");
                        }
                        this._includeList.Add(i);
                    }
                }
            }
            if (excludeList != null && excludeList.Trim().Length > 0)
            {
                excludeList = excludeList.ToLower();
                string[] excludeItems = excludeList.Split(new char[]
				{
					','
				});
                string[] array = excludeItems;
                for (int j = 0; j < array.Length; j++)
                {
                    string excludeItem = array[j];
                    this._excludeList.Add(excludeItem.Trim());
                }
            }
        }
        public bool IncludeFile(string relativeDirectoryPath)
        {
            string normalizedFile = relativeDirectoryPath.ToLower();

            if (this.EntryNameInList(this._includeList, normalizedFile))
            {
                return true;
            }
            else if (this.EntryNameInList(this._excludeList, normalizedFile))
                {
                    return false;
                }
            else
            {
                return true;
            }

            //return this.EntryNameInList(this._includeList, normalizedFile) && !this.EntryNameInList(this._excludeList, normalizedFile);
        }
        public List<string> GetIncludedFilesFromPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("Directory \"" + directoryPath + "\" does not exist.");
            }
            int adjustedRootDirLength = this.StripTrailingSlash(directoryPath).Length + 1;
            List<string> includeList = new List<string>();
            this.AppendIncludeList(includeList, adjustedRootDirLength, directoryPath);
            return includeList;
        }
        public List<string> GetExcludedFilesFromPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("Directory \"" + directoryPath + "\" does not exist.");
            }
            int adjustedRootDirLength = this.StripTrailingSlash(directoryPath).Length + 1;
            List<string> excludeList = new List<string>();
            this.AppendExcludeList(excludeList, adjustedRootDirLength, directoryPath);
            return excludeList;
        }
        private string StripTrailingSlash(string path)
        {
            while (path.EndsWith("\\"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }
        private string NormalizePath(int adjustedRootDirLength, string path)
        {
            return path.Substring(adjustedRootDirLength).ToLower();
        }
        private string NormalizeDirectory(int adjustedRootDirLength, string directoryPath)
        {
            return this.NormalizePath(adjustedRootDirLength, directoryPath) + "\\";
        }
        private void AppendIncludeList(List<string> includeList, int adjustedRootDirLength, string directoryPath)
        {
            string[] array = Directory.GetFiles(directoryPath);
            for (int i = 0; i < array.Length; i++)
            {
                string file = array[i];
                string normalizedFile = this.NormalizePath(adjustedRootDirLength, file);
                if (this.EntryNameInList(this._includeList, normalizedFile) && !this.EntryNameInList(this._excludeList, normalizedFile))
                {
                    includeList.Add(file);
                }
            }
            array = Directory.GetDirectories(directoryPath);
            for (int i = 0; i < array.Length; i++)
            {
                string subdirectory = array[i];
                string normalizedDirectory = this.NormalizeDirectory(adjustedRootDirLength, subdirectory);
                if (!this.EntryNameInList(this._excludeList, normalizedDirectory))
                {
                    this.AppendIncludeList(includeList, adjustedRootDirLength, subdirectory);
                }
            }
        }
        private void AppendExcludeList(List<string> excludeList, int adjustedRootDirLength, string directoryPath)
        {
            string[] array = Directory.GetFiles(directoryPath);
            for (int i = 0; i < array.Length; i++)
            {
                string file = array[i];
                string normalizedFile = this.NormalizePath(adjustedRootDirLength, file);
                if (!this.EntryNameInList(this._includeList, normalizedFile) || this.EntryNameInList(this._excludeList, normalizedFile))
                {
                    excludeList.Add(file);
                }
            }
            array = Directory.GetDirectories(directoryPath);
            for (int i = 0; i < array.Length; i++)
            {
                string subdirectory = array[i];
                string normalizedDirectory = this.NormalizeDirectory(adjustedRootDirLength, subdirectory);
                if (this.EntryNameInList(this._excludeList, normalizedDirectory))
                {
                    this.AppendAllFilesToList(excludeList, subdirectory);
                }
                else
                {
                    this.AppendExcludeList(excludeList, adjustedRootDirLength, subdirectory);
                }
            }
        }
        public void AppendAllFilesToList(List<string> list, string directoryPath)
        {
            string[] array = Directory.GetFiles(directoryPath);
            for (int i = 0; i < array.Length; i++)
            {
                string file = array[i];
                list.Add(file);
            }
            array = Directory.GetDirectories(directoryPath);
            for (int i = 0; i < array.Length; i++)
            {
                string subdirectory = array[i];
                this.AppendAllFilesToList(list, subdirectory);
            }
        }
        private bool EntryNameInList(List<string> matchList, string normalizedEntry)
        {
            bool result;
            foreach (string matchListItem in matchList)
            {
                string matchItem = matchListItem;
                if (matchListItem.StartsWith(".\\"))
                {
                    if (normalizedEntry.IndexOf("\\") > -1)
                    {
                        result = false;
                        return result;
                    }
                    matchItem = matchListItem.Substring(2);
                }
                if (matchItem.Equals("*"))
                {
                    result = true;
                    return result;
                }
                if (matchItem.StartsWith("*") && matchItem.EndsWith("*"))
                {
                    string chompedMatchItem = matchItem.Substring(1, matchItem.Length - 2);
                    if (normalizedEntry.IndexOf(chompedMatchItem, StringComparison.CurrentCultureIgnoreCase) > -1)
                    {
                        result = true;
                        return result;
                    }
                }
                else
                {
                    if (matchItem.EndsWith("*"))
                    {
                        string chompedMatchItem = matchItem.Substring(0, matchItem.Length - 1);
                        if (normalizedEntry.StartsWith(chompedMatchItem, StringComparison.CurrentCultureIgnoreCase))
                        {
                            result = true;
                            return result;
                        }
                    }
                    else
                    {
                        if (matchItem.StartsWith("*"))
                        {
                            string chompedMatchItem = matchItem.Substring(1, matchItem.Length - 1);
                            if (normalizedEntry.EndsWith(chompedMatchItem, StringComparison.CurrentCultureIgnoreCase))
                            {
                                result = true;
                                return result;
                            }
                        }
                        else
                        {
                            if (matchItem.Equals(normalizedEntry, StringComparison.CurrentCultureIgnoreCase))
                            {
                                result = true;
                                return result;
                            }
                        }
                    }
                }
            }
            result = false;
            return result;
        }
    }
}
