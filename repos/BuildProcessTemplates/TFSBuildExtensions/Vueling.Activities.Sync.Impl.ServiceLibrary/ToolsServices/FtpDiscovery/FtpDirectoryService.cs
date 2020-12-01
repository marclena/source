using Vueling.Activities.Sync.Impl.ServiceLibrary.Helpers;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TFSBuildExtensions.Library.SynchronizeContent;
using Vueling.Extensions.Library.DI;
using System.Text;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.FtpDiscovery
{
    [RegisterService]
    public class FtpDirectoryService : IFtpDirectoryService
    {
        public List<DirectoryContent> GetDirectoryInfo(FtpSettings ftpSettings, List<string> excludeContent)
        {
            var ftpContent = new List<DirectoryContent>();

            RecursiveGetFTPListing(ftpSettings, "", ftpContent);

            return ftpContent;
        }

        public bool ExistsStartingFolder(FtpSettings ftpSettings)
        {
            if (string.IsNullOrEmpty(ftpSettings.StartingPath) || ftpSettings.StartingPath.Equals("/")) { return true; }

            try
            {
                var dirInfo = GetFTPListing(ftpSettings, "");

                if (dirInfo.Count == 0) { return false; }

                return dirInfo.Where(x =>
                    x.AccessPath.Equals(ftpSettings.StartingPath) || x.AccessPath.StartsWith(ftpSettings.StartingPath + "/"))
                    .Any();
            }
            catch 
            { 
                return false; 
            }
        }

        private void RecursiveGetFTPListing(FtpSettings ftpSettings, string relativePath, List<DirectoryContent> ftpContent)
        {
            var dirInfo = GetFTPListing(ftpSettings, relativePath);

            ftpContent.AddRange(dirInfo);

            foreach (var item in dirInfo.Where(x => x.IsDirectory))
            {
                RecursiveGetFTPListing(ftpSettings, item.RelativePath, ftpContent);
            }
        }

        public List<DirectoryContent> GetFTPListing(FtpSettings ftpSettings, string relativePath)
        {
            List<DirectoryContent> dirContent = new List<DirectoryContent>();

            string[] results = GetFtpContent(ftpSettings, relativePath);

            Regex regexUnixStyle = GetRegexUnixStyle();

            Regex regexWinStyle = GetRegexWinStyle();

            foreach (var parsed in results)
            {
                if (IsUnixFtp(regexWinStyle, parsed))
                {
                    BuildUnixDirectoryInfo(regexUnixStyle, parsed, relativePath, dirContent);
                }
                else
                {
                    BuildWindowsDirectoryInfo(regexWinStyle, parsed, ftpSettings.StartingPath, relativePath, dirContent);
                }
            }
            

            return dirContent;
        }

        private Regex GetRegexWinStyle()
        {
            Regex regexWinStyle = new Regex(

                                     @"^" +                          //# Start of line
                                     @"(?<dir>[\-ld])" +             //# File size         
                                     @"(?<permission>[\-rwx]{9})" +  //# Whitespace          \n
                                     @"\s+" +                        //# Whitespace
                                     @"(?<filecode>\d+)" +
                                     @"\s+" +                        //# Whitespace
                                     @"(?<owner>\w+)" +
                                     @"\s+" +                        //# Whitespace          \n
                                     @"(?<group>\w+)" +
                                     @"\s+" +                        //# Whitespace          \n
                                     @"(?<size>\d+)" +
                                     @"\s+" +                        //# Whitespace          \n
                                     @"(?<month>\w{3})" +            //# Month (3 letters)   \n
                                     @"\s+" +                        //# Whitespace          \n
                                     @"(?<day>\d{1,2})" +            //# Day (1 or 2 digits) \n
                                     @"\s+" +                        //# Whitespace          \n
                                     @"(?<timeyear>[\d:]{4,5})" +    //# Time or year        \n
                                     @"\s+" +                        //# Whitespace          \n
                                     @"(?<filename>(.*))" +       //# Filename            \n
                                     @"$", RegexOptions.Compiled);
            return regexWinStyle;
        }

        private Regex GetRegexUnixStyle()
        {
            return new Regex(@"^(?<month>\d{1,2})-(?<day>\d{1,2})-(?<year>\d{1,2})\s+(?<hour>\d{1,2}):(?<minutes>\d{1,2})(?<ampm>am|pm)\s+(?<dir>[<]dir[>])?\s+(?<size>\d+)?\s+(?<filename>.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private string[] GetFtpContent(FtpSettings ftpSettings, string relativePath)
        {
            StringBuilder result = new StringBuilder();


            var startingPath = ftpSettings.StartingPath.EndsWith("/") ? ftpSettings.StartingPath.TrimEnd('/') : ftpSettings.StartingPath;

            var hiddenCommand = ftpSettings.ShowHiddenFiles ? "-a" : string.Empty;

            var relative = relativePath.EndsWith("/") ? relativePath + hiddenCommand : relativePath + "/" + hiddenCommand;

            var uriBuilder = new UriBuilder("ftp", ftpSettings.ServerAddress.Replace("ftp://", ""), 21, startingPath + relative);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uriBuilder.Uri);

            request.Credentials = new NetworkCredential(ftpSettings.User, ftpSettings.Password);

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            FtpWebResponse response1 = (FtpWebResponse)request.GetResponse();

            StreamReader readerStream = new StreamReader(response1.GetResponseStream());

            string line1 = readerStream.ReadLine();

            if (string.IsNullOrEmpty(line1)) { return new string[] { }; }

            while (!string.IsNullOrEmpty(line1))
            {
                if (line1.EndsWith(" .") || line1.EndsWith(" .."))
                {
                    line1 = readerStream.ReadLine();
                    continue;
                }

                result.Append(line1);

                result.Append("\n");

                line1 = readerStream.ReadLine();
            }

            if (result.Length == 0) { return new string[] { }; }

            result.Remove(result.ToString().LastIndexOf('\n'), 1);

            var results = result.ToString().Split('\n');

            return results;
        }

        private void BuildWindowsDirectoryInfo(Regex regexWinStyle, string parsed, string startingPath, string relativePath, List<DirectoryContent> dirContent)
        {
            //WINDOWS
            var split1 = regexWinStyle.Match(parsed);

            string FormatVersion = "WINDOWS";

            string IsDirWin = split1.Groups["dir"].ToString();

            string FileName = split1.Groups["filename"].ToString();//GETTING FILENAME EXAMPLE

            string size = split1.Groups["size"].ToString();

            string day = split1.Groups["day"].ToString();
            string month = split1.Groups["month"].ToString();
            string date = split1.Groups["day"].ToString() + "-" + split1.Groups["month"].ToString();

            string timeyear = split1.Groups["timeyear"].ToString();

            string time = "12:00";
            int year = DateTime.UtcNow.Year;

            if (timeyear.Contains(":")) { time = timeyear; }
            else { year = Convert.ToInt32(timeyear); }

            dirContent.Add(new DirectoryContent
            {
                AccessPath = startingPath + relativePath + "/" + FileName,
                RelativePath = relativePath + "/" + FileName,
                FileSize = size,
                IsDirectory = IsDirWin.Equals("d", StringComparison.InvariantCultureIgnoreCase),
                LastWrite = DateTime.Parse(string.Format("{0}-{1}-{2} {3}", year, month, day, time))
            });
        }

        //TODO: test this method!!!!
        private void BuildUnixDirectoryInfo(Regex regexUnixStyle, string parsed, string startingPath, List<DirectoryContent> dirContent)
        {
            //UNIX

            var split2 = regexUnixStyle.Match(parsed);

            string IsDirUni = split2.Groups["dir"].ToString();

            string FileName = split2.Groups["filename"].ToString();//GETTING FILENAME EXAMPLE

            if (IsDirUni.Equals("<DIR>", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!FileName.EndsWith("."))
                {
                    //CREATE DIRECTORY LIST

                    string FormatVersion = "UNIX";

                    string date = split2.Groups["day"].ToString() + "-" + split2.Groups["month"].ToString() + "-" + split2.Groups["year"].ToString();

                    //string time = split2.Groups["hour"].ToString() + ":" + split2.Groups["minutes"].ToString() + split2.Groups["ampm"].ToString();
                    string time = split2.Groups["hour"].ToString() + ":" + split2.Groups["minutes"].ToString();

                    string Size = "DIR";

                    string FolderName = split2.Groups["filename"].ToString();

                    dirContent.Add(new DirectoryContent
                    {
                        AccessPath = startingPath + "/" + FileName,
                        RelativePath = startingPath + "/" + FileName,
                        FileSize = "0",
                        IsDirectory = true,
                        LastWrite = DateTime.Parse(string.Format("{0} {1}", date, time))
                    });
                }
            }
            else
            {
                //CREATE FILE LIST

                string FormatVersion = "UNIX";

                string date = split2.Groups["day"].ToString() + "-" + split2.Groups["month"].ToString() + "-" + split2.Groups["year"].ToString();

                string time = split2.Groups["hour"].ToString() + ":" + split2.Groups["minutes"].ToString() + split2.Groups["ampm"].ToString();

                string size = split2.Groups["size"].ToString();

                string FolderName = split2.Groups["filename"].ToString();

                dirContent.Add(new DirectoryContent
                {
                    AccessPath = startingPath + "/" + FileName,
                    RelativePath = startingPath + "/" + FileName,
                    FileSize = size,
                    IsDirectory = false,
                    LastWrite = DateTime.Parse(string.Format("{0} {1}", date, time))
                });
            }
        }

        private bool IsUnixFtp(Regex regexWinStyle, string parsed)
        {
            var split = regexWinStyle.Match(parsed);

            string fileNAAm = split.Groups["filename"].ToString();//CHECKS WHAT FTP DETAILS FORMAT IS IN: UNIX || WINDOWS

            return fileNAAm.Length < 1;
        }
    }
}
