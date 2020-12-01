using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Vueling.Activities.Contracts.ServiceLibrary.Compression;
using Vueling.Activities.Impl.ServiceLibrary.Compression;

namespace TFSBuildExtensions.AWS
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class UploadAWSPackageToS3 : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }

        [RequiredArgument]
        public InArgument<List<string>> ProjectsToIncludeInPackage { get; set; }

        [RequiredArgument]
        public InArgument<string> WebSite { get; set; }

        [DefaultValue(false)]
        public InArgument<bool> RecreateApplication { get; set; }

        [DefaultValue("vueling-releases")]
        public InArgument<string> S3Bucket { get; set; }

        [DefaultValue(false)]
        public InArgument<bool> S3InSeparateFiles { get; set; }

        public InArgument<string> AwsProfile { get; set; }

        protected override void InternalExecute()
        {
            initialize();

            CreateXmlFileDefinition xmlFileDefinition = new CreateXmlFileDefinition(this.BuildDetail.BuildDefinition.Name,
                                                                                    this.BuildDetail.BuildNumber,
                                                                                    this.WebSite.Get(this.ActivityContext),
                                                                                    this.BinariesDirectory.Get(this.ActivityContext),
                                                                                    this.ProjectsToIncludeInPackage.Get(this.ActivityContext),
                                                                                    this.RecreateApplication.Get(this.ActivityContext));
            xmlFileDefinition.createXmlFile();

            CreatePackage7z createPackage7z = new CreatePackage7z();
            createPackage7z.CreatePackageFile(this.ProjectsToIncludeInPackage.Get(this.ActivityContext), this.BinariesDirectory.Get(this.ActivityContext));

            bool s3InSeparateFiles = false;
            if (S3InSeparateFiles != null && S3InSeparateFiles.Get(this.ActivityContext) != null)
                s3InSeparateFiles = S3InSeparateFiles.Get(this.ActivityContext); 
            List<string> files = CreateFiles(this.BinariesDirectory.Get(this.ActivityContext), s3InSeparateFiles);

            CopyToS3(files);
        }

        internal void initialize()
        {
            if (Directory.Exists(this.BinariesDirectory.Get(this.ActivityContext) + @"\AWS"))
            {
                DeleteContentFolder(this.BinariesDirectory.Get(this.ActivityContext) + @"\AWS");
            }
        }

        internal void DeleteContentFolder(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;

                file.Delete();
            }
            foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
            {
                DeleteContentFolder(folder.FullName);
            }

            directoryInfo.Delete();
        }


        internal List<string> CreateFiles(string binariesDirectory, bool s3InSeparateFiles)
        {
            List<string> zipFiles = new List<string>();
            List<string> pathsToCompress = new List<string>();

            if (s3InSeparateFiles)
            {
                var directories = Directory.GetDirectories(binariesDirectory + @"\AWS\");
                pathsToCompress.AddRange(directories);
            }
            else
            {
                pathsToCompress.Add(binariesDirectory + @"\AWS\*");
            }

            IZipService zipService = new ZipService();

            foreach (var pathPackageFiles in pathsToCompress)
            {
                string destinationFile = Path.Combine(binariesDirectory, BuildDetail.BuildNumber + ".zip");
                if (s3InSeparateFiles)
                {
                    this.LogBuildMessage(string.Format("Generating zip for {0}", pathPackageFiles));
                    
                    destinationFile = Path.Combine(binariesDirectory, Path.GetFullPath(pathPackageFiles).Split('\\').Last() + ".zip");
                }
                zipService.Initialize(destinationFile, new List<string> { pathPackageFiles }, new List<string>());
                zipService.InternalExecute();
                zipFiles.Add(destinationFile);
            }

            return zipFiles;
        }


        internal void CopyToS3(List<string> filesToCopy)
        {
            string awsProfile = string.Empty;
            if (AwsProfile != null && AwsProfile.Get(this.ActivityContext) != null && !string.IsNullOrEmpty(AwsProfile.Get(this.ActivityContext)))
            {
                awsProfile = " --profile " + AwsProfile.Get(this.ActivityContext);
            }
            foreach (var filename in filesToCopy)
            {
                string s3Bucket = "vueling-releases";
                if (S3Bucket != null && S3Bucket.Get(this.ActivityContext) != null && !string.IsNullOrEmpty(S3Bucket.Get(this.ActivityContext)))
                {
                    s3Bucket = S3Bucket.Get(this.ActivityContext);
                }
                this.LogBuildMessage(string.Format("Deploying to S3 bucket '{0}' application/file '{1}'", s3Bucket, filename));
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\Amazon\AWSCLI\aws.exe",
                    Arguments = "s3 cp " + filename + " s3://" + s3Bucket + awsProfile,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = this.BinariesDirectory.Get(this.ActivityContext),
                    CreateNoWindow = true
                };

                RunCommandLineProcess(processStartInfo);
            }
        }

        private void RunCommandLineProcess(ProcessStartInfo processStartInfo)
        {
            using (Process process = Process.Start(processStartInfo))
            {
                using (ManualResetEvent mreOut = new ManualResetEvent(false), mreErr = new ManualResetEvent(false))
                {
                    process.OutputDataReceived += (o, e) =>
                    {
                        if (e.Data == null)
                        {
                            mreOut.Set();
                        }
                        else
                        {
                            this.LogBuildMessage(e.Data);
                        }
                    };
                    process.BeginOutputReadLine();
                    process.ErrorDataReceived += (o, e) =>
                    {
                        if (e.Data == null)
                        {
                            mreErr.Set();
                        }
                        else
                        {
                            this.LogBuildMessage(e.Data);
                        }
                    };
                    process.BeginErrorReadLine();
                    process.StandardInput.Close();
                    process.WaitForExit();

                    mreOut.WaitOne();
                    mreErr.WaitOne();

                    if (process.ExitCode != 0)
                    {
                        this.LogBuildError(process.ExitCode.ToString(CultureInfo.CurrentCulture));
                    }
                }
            }
        }
    }
}
