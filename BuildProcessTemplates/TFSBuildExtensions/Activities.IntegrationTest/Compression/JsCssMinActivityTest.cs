using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Activities.IntegrationTest.Compression
{
    [TestClass]
    public class JsCssMinActivityTest
    {
        [TestMethod]
        public void CompressSkySalesVuelingjs()
        {
            var target = new TFSBuildExtensions.Compression.JsCssMinActivity
            {
                FullPathVuelingMinifierConsoleUI = @"D:\workspaces\tfs\BuildProcessTemplate\Vueling.Minifier.ConsoleUI\Vueling.Minifier.ConsoleUI.exe",
                Folder = @"D:\workspaces\tfs\Vueling.StaticContent\PRE\Vueling.StaticContent.WebUI\skysales\js",
                CompresscssFile = false,
                CompressjsFile = true
            };

            var parameters = new Dictionary<string, object>
            { { "ExcludedFiles",new string[]{"TravelDocumentInput.js"} }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void CompressSkySalesVuelingcss()
        {
            var target = new TFSBuildExtensions.Compression.JsCssMinActivity
            {
                FullPathVuelingMinifierConsoleUI = @"D:\workspaces\tfs\BuildProcessTemplate\Vueling.Minifier.ConsoleUI\Vueling.Minifier.ConsoleUI.exe",
                Folder = @"D:\workspaces\tfs\Vueling.StaticContent\PRE\Vueling.StaticContent.WebUI\skysales\css",
                CompresscssFile = true,
                CompressjsFile = false
            };

            var parameters = new Dictionary<string, object>
            { { "ExcludedFiles",new string[]{} }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void CompressCMSVuelingjs()
        {
            var target = new TFSBuildExtensions.Compression.JsCssMinActivity
            {
                FullPathVuelingMinifierConsoleUI = @"D:\workspaces\tfs\BuildProcessTemplate\Vueling.Minifier.ConsoleUI\Vueling.Minifier.ConsoleUI.exe",
                Folder = @"D:\workspaces\tfs\Vueling.StaticContent.CMS\PRE\Vueling.StaticContent.CMS.WebUI\cms\scripts",
                CompresscssFile = false,
                CompressjsFile = true
            };

            var parameters = new Dictionary<string, object>
            { { "ExcludedFiles",new string[]{} }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void CompressCMSVuelingcss()
        {
            var target = new TFSBuildExtensions.Compression.JsCssMinActivity
            {
                FullPathVuelingMinifierConsoleUI = @"D:\workspaces\tfs\BuildProcessTemplate\Vueling.Minifier.ConsoleUI\Vueling.Minifier.ConsoleUI.exe",
                Folder = @"D:\workspaces\tfs\Vueling.StaticContent.CMS\PRE\Vueling.StaticContent.CMS.WebUI\cms\css",
                CompresscssFile = true,
                CompressjsFile = false
            };

            var parameters = new Dictionary<string, object>
            { { "ExcludedFiles",new string[]{} }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void CompressVuelingPuntoWebUI()
        {
            string folderBase = "C:\\Repositorio_Web";

            var target = new TFSBuildExtensions.Compression.JsCssMinActivity
            {
                FullPathVuelingMinifierConsoleUI = folderBase + "\\Vueling.Build.Javascript.Minifier.ConsoleUI\\Vueling.Build.Javascript.Minifier.ConsoleUI.exe",
                Folder = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\fake\Vueling.Punto.WebUI"),
                CompresscssFile = true,
                CompressjsFile = true
            };

            var parameters = new Dictionary<string, object>
            { { "ExcludedFiles",new string[]{".min.", "ext-"} }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.IsNotNull(output); 
        }
    }
}
