using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.IO;

namespace Activities.UnitTest.Ftp
{
    [TestClass]
    public class FtpActivityTest
    {
        [TestMethod]
        public void SyncStaticContentFilesPRE()
        {
            string sourcesDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\fake\\Vueling.SkySales.StaticContent.JS.WebUI";
            string ProjectStaticContentName = "Vueling.SkySales.StaticContent.JS.WebUI";

            var target = new TFSBuildExtensions.Ftp.FtpActivity
            {
                SourcesDirectory = sourcesDirectory,
                FtpFolderPath = @"\" + ProjectStaticContentName,
                FtpUserName = "staticcontent_pre",
                FtpPassword = "0i8ruTGFLA",
                FtpSite = "ftp.vueling.com",
                FileMask = ".*,*.csproj,*.csproj.user,*.csproj.vspscc,Web.Release.config,Web.Debug.config,Web.config,EnsureNugetPackages.*,ExecutePrebuildTasks.*"
            };

            var invoker = new WorkflowInvoker(target);

            try
            {
                var output = invoker.Invoke();
                Assert.AreEqual(false, output["Result"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
