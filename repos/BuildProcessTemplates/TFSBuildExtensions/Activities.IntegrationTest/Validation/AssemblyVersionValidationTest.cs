using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.UnitTest.Validation
{
    [TestClass]
    public class AssemblyVersionValidationTest
    {
        [TestMethod]
        public void Valid_EnvironmentDifferentFromPRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\Vueling.XXX\PRE",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\Vueling\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.XXX.Contracts.ServiceLibrary", "Vueling.XXX.DB.Infrastructure", "Vueling.XXX.DB.Infrastructure.UnitTest", "Vueling.XXX.Impl.ServiceLibrary", "Vueling.XXX.Impl.ServiceLibrary.UnitTest", "Vueling.XXX.Library", "Vueling.XXX.Library.UnitTest", "Vueling.XXX.Message", "Vueling.XXX.Model", "Vueling.XXX.Publisher.Contracts.ServiceLibrary", "Vueling.XXX.Publisher.Impl.ServiceLibrary", "Vueling.XXX.Publisher.WCF.WebService", "Vueling.XXX.Publisher.Subscriber.WindowsService", "Vueling.XXX.WCF.REST.WebService", "Vueling.XXX.WCF.REST.WebService.UnitTest", "Vueling.XXX.WCF.REST.WebService.IntegrationTest", "Vueling.XXX.WCF.WebService", "Vueling.XXX.WCF.WebService.UnitTest", "Vueling.XXX.WebUI", "Vueling.XXX.WebUI.UnitTest" } },
                { "AssembliesToProcess", new List<string> {"Vueling"}},
                { "AssemblyExceptions", new List<string> {"Vueling.Configuration.Library", "Vueling.Extensions.Library", "Vueling.Logging.Library", "Vueling.Messaging.Library", "Vueling.Threading.Library", "Vueling.Authorization.Library", "Vueling.Web.Library", "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling_XXX.Database" } }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_OnlyVuelingAssembliesToProcess_WithAssemblyExceptions()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.XXX\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.XXX.Contracts.ServiceLibrary", "Vueling.XXX.DB.Infrastructure", "Vueling.XXX.DB.Infrastructure.UnitTest", "Vueling.XXX.Impl.ServiceLibrary", "Vueling.XXX.Impl.ServiceLibrary.UnitTest", "Vueling.XXX.Library", "Vueling.XXX.Library.UnitTest", "Vueling.XXX.Message", "Vueling.XXX.Model", "Vueling.XXX.Publisher.Contracts.ServiceLibrary", "Vueling.XXX.Publisher.Impl.ServiceLibrary", "Vueling.XXX.Publisher.WCF.WebService", "Vueling.XXX.Publisher.Subscriber.WindowsService", "Vueling.XXX.WCF.REST.WebService", "Vueling.XXX.WCF.REST.WebService.UnitTest", "Vueling.XXX.WCF.REST.WebService.IntegrationTest", "Vueling.XXX.WCF.WebService", "Vueling.XXX.WCF.WebService.UnitTest", "Vueling.XXX.WebUI", "Vueling.XXX.WebUI.UnitTest" } },
                { "AssembliesToProcess", new List<string> {"Vueling"}},
                { "AssemblyExceptions", new List<string> {"Vueling.Configuration.Library", "Vueling.Extensions.Library", "Vueling.Logging.Library", "Vueling.Messaging.Library", "Vueling.Threading.Library", "Vueling.Authorization.Library", "Vueling.Web.Library", "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling_XXX.Database" } }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["AreVersionValid"]);
        }

        [TestMethod]
        public void Invalid_OnlyVuelingAssembliesToProcess_WithoutAssemblyExceptions()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.XXX\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling.XXX.Library", "Vueling.XXX.Library.UnitTest", "Vueling.XXX.ServiceLibrary", "Vueling.XXX.ServiceLibrary.UnitTest", "Vueling.XXX.WCF.REST.WebService", "Vueling.XXX.WCF.REST.WebService.UnitTest", "Vueling.XXX.WCF.WebService", "Vueling.XXX.WCF.WebService.UnitTest", "Vueling.XXX.WCF.WindowsService", "Vueling.XXX.WCF.WindowsService.IntegrationTest", "Vueling.XXX.WCF.WindowsService.UnitTest", "Vueling.XXX.WebService", "Vueling.XXX.WebUI", "Vueling.XXX.WebUI.UnitTest", "Vueling_XXX.Database" } },
                { "AssembliesToProcess", new List<string> {"Vueling"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(false, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Invalid_OnlyvuelingAssemblyExceptions_WithoutAssembliesToProcess()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.XXX\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling.XXX.Library", "Vueling.XXX.Library.UnitTest", "Vueling.XXX.ServiceLibrary", "Vueling.XXX.ServiceLibrary.UnitTest", "Vueling.XXX.WCF.REST.WebService", "Vueling.XXX.WCF.REST.WebService.UnitTest", "Vueling.XXX.WCF.WebService", "Vueling.XXX.WCF.WebService.UnitTest", "Vueling.XXX.WCF.WindowsService", "Vueling.XXX.WCF.WindowsService.IntegrationTest", "Vueling.XXX.WCF.WindowsService.UnitTest", "Vueling.XXX.WebService", "Vueling.XXX.WebUI", "Vueling.XXX.WebUI.UnitTest", "Vueling_XXX.Database" } },
                { "AssemblyExceptions", new List<string> {"Vueling.Configuration.Library", "Vueling.Extensions.Library", "Vueling.Logging.Library", "Vueling.Messaging.Library", "Vueling.Threading.Library", "Vueling.Authorization.Library", "Vueling.Web.Library", "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling_XXX.Database"} }
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(false, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_NotificationMail_INTEnvironment()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.XXX\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling.XXX.Library", "Vueling.XXX.Library.UnitTest", "Vueling.XXX.ServiceLibrary", "Vueling.XXX.ServiceLibrary.UnitTest", "Vueling.XXX.WCF.REST.WebService", "Vueling.XXX.WCF.REST.WebService.UnitTest", "Vueling.XXX.WCF.WebService", "Vueling.XXX.WCF.WebService.UnitTest", "Vueling.XXX.WCF.WindowsService", "Vueling.XXX.WCF.WindowsService.IntegrationTest", "Vueling.XXX.WCF.WindowsService.UnitTest", "Vueling.XXX.WebService", "Vueling.XXX.WebUI", "Vueling.XXX.WebUI.UnitTest", "Vueling_XXX.Database" } },
                { "AssemblyExceptions", new List<string> {"Vueling.Configuration.Library", "Vueling.Extensions.Library", "Vueling.Logging.Library", "Vueling.Messaging.Library", "Vueling.Threading.Library", "Vueling.Authorization.Library", "Vueling.Web.Library", "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling_XXX.Database"} }
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_Residents_INTEnvironment()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Residents\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Resident.ServiceLibrary.UnitTest", "Vueling.Residents.Infrastructure", "Vueling.Residents.Library", "Vueling.Residents.Library.UnitTest", "Vueling.Residents.ServiceLibrary", "Vueling.Residents.ServiceLibrary.IntegrationTest", "Vueling.Residents.ServiceLibrary.UnitTest", "Vueling.Residents.ValidationSSR.Library", "Vueling.Residents.ValidationSSR.ServiceLibrary", "Vueling.Residents.ValidationSSR.WebUI", "Vueling.Residents.WCF.WebService", "Vueling.Residents.WCF.WindowsService", "Vueling.Residents.WebUI", "Vueling_Residents.Database" } },
                { "AssemblyExceptions", new List<string> {"Vueling.Configuration.Library", "Vueling.Extensions.Library", "Vueling.Logging.Library", "Vueling.Messaging.Library", "Vueling.Threading.Library", "Vueling.Authorization.Library", "Vueling.Web.Library", "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling_XXX.Database"} }
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);            
        }

        [TestMethod]
        public void Valid_NewsLetter_PREEnvironment()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.NewsLetter\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.NewsLetter.ServiceLibrary" } },
                {"AssembliesToProcess", new List<string> {"Vueling"}},
                { "AssemblyExceptions", new List<string> {"Vueling.AppFabricCache.ServiceLibrary", "Autofac", "Vueling.Configuration.Library", "Vueling.Extensions.Library", "Vueling.Logging.Library", "Vueling.Messaging.Library", "Vueling.Threading.Library", "Vueling.Authorization.Library", "Vueling.Web.Library", "Vueling.XXX.CustomControl", "Vueling.XXX.CustomController", "Vueling.XXX.CustomModel", "Vueling.XXX.CustomNotification", "Vueling.XXX.CustomRule", "Vueling.XXX.Installer", "Vueling_XXX.Database"} }
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);                        
        }

        [TestMethod]
        public void Valid_VuelingCheckin_DEV()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Checkin\DEV",
                Environment = "DEV",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Checkin.WebUI", "Vueling.DeferredCheckin.Publisher.WebService", "Vueling.DeferredCheckin.Publisher.WebService.UnitTest", "Vueling.DeferredCheckin.WCF.WebService", "Vueling.DeferredCheckin.WCF.WebService.IntegrationTest", "Vueling.DeferredCheckin.WCF.WebService.UnitTest", "Vueling.DeferredFeed.WCF.WebService", "Vueling.DeferredFeed.WCF.WebService.IntegrationTest", "Vueling.DeferredFeed.WCF.WebService.UnitTest", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary.IntegrationTest", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary.UnitTest", "Vueling.Checkin.DTO", "Vueling.Checkin.InventoryRepository.Impl.ServiceLibrary", "Vueling.Checkin.Operation.Contracts.ServiceLibrary", "Vueling.Checkin.Operation.Impl.ServiceLibrary", "Vueling.Checkin.Operation.Impl.ServiceLibrary.UnitTest", "Vueling.Checkin.Validation.Contracts.ServiceLibrary", "Vueling.Checkin.Validation.Impl.ServiceLibrary", "Vueling.Checkin.Validation.Impl.ServiceLibrary.UnitTest", "Vueling.DeferredCheckin.Contracts.ServiceLibrary", "Vueling.DeferredCheckin.Impl.ServiceLibrary", "Vueling.DeferredCheckin.Impl.ServiceLibrary.UnitTest", "Vueling.DeferredCheckin.Publisher.Message", "Vueling.Checkin.Library", "Vueling.Checkin.Library.UnitTest", "Vueling.DeferredCheckin.Publisher.Library", "Vueling.DeferredCheckin.Publisher.Library.UnitTest", "Vueling.DeferredCheckin.Infrastructure" } },
                {"AssembliesToProcess", new List<string> {"Vueling"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingCheckin_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Checkin\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Checkin.WebUI", "Vueling.DeferredCheckin.Publisher.WebService", "Vueling.DeferredCheckin.Publisher.WebService.UnitTest", "Vueling.DeferredCheckin.WCF.WebService", "Vueling.DeferredCheckin.WCF.WebService.IntegrationTest", "Vueling.DeferredCheckin.WCF.WebService.UnitTest", "Vueling.DeferredFeed.WCF.WebService", "Vueling.DeferredFeed.WCF.WebService.IntegrationTest", "Vueling.DeferredFeed.WCF.WebService.UnitTest", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary.IntegrationTest", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary.UnitTest", "Vueling.Checkin.DTO", "Vueling.Checkin.InventoryRepository.Impl.ServiceLibrary", "Vueling.Checkin.Operation.Contracts.ServiceLibrary", "Vueling.Checkin.Operation.Impl.ServiceLibrary", "Vueling.Checkin.Operation.Impl.ServiceLibrary.UnitTest", "Vueling.Checkin.Validation.Contracts.ServiceLibrary", "Vueling.Checkin.Validation.Impl.ServiceLibrary", "Vueling.Checkin.Validation.Impl.ServiceLibrary.UnitTest", "Vueling.DeferredCheckin.Contracts.ServiceLibrary", "Vueling.DeferredCheckin.Impl.ServiceLibrary", "Vueling.DeferredCheckin.Impl.ServiceLibrary.UnitTest", "Vueling.DeferredCheckin.Publisher.Message", "Vueling.Checkin.Library", "Vueling.Checkin.Library.UnitTest", "Vueling.DeferredCheckin.Publisher.Library", "Vueling.DeferredCheckin.Publisher.Library.UnitTest", "Vueling.DeferredCheckin.Infrastructure" } },
                {"AssembliesToProcess", new List<string> {"Vueling"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingCheckin_PRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\Vueling.Checkin\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\Vueling\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Checkin.WebUI", "Vueling.DeferredCheckin.Publisher.WebService", "Vueling.DeferredCheckin.Publisher.WebService.UnitTest", "Vueling.DeferredCheckin.WCF.WebService", "Vueling.DeferredCheckin.WCF.WebService.IntegrationTest", "Vueling.DeferredCheckin.WCF.WebService.UnitTest", "Vueling.DeferredFeed.WCF.WebService", "Vueling.DeferredFeed.WCF.WebService.IntegrationTest", "Vueling.DeferredFeed.WCF.WebService.UnitTest", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary.IntegrationTest", "Vueling.Checkin.BusinessCache.Impl.ServiceLibrary.UnitTest", "Vueling.Checkin.DTO", "Vueling.Checkin.InventoryRepository.Impl.ServiceLibrary", "Vueling.Checkin.Operation.Contracts.ServiceLibrary", "Vueling.Checkin.Operation.Impl.ServiceLibrary", "Vueling.Checkin.Operation.Impl.ServiceLibrary.UnitTest", "Vueling.Checkin.Validation.Contracts.ServiceLibrary", "Vueling.Checkin.Validation.Impl.ServiceLibrary", "Vueling.Checkin.Validation.Impl.ServiceLibrary.UnitTest", "Vueling.DeferredCheckin.Contracts.ServiceLibrary", "Vueling.DeferredCheckin.Impl.ServiceLibrary", "Vueling.DeferredCheckin.Impl.ServiceLibrary.UnitTest", "Vueling.DeferredCheckin.Publisher.Message", "Vueling.Checkin.Library", "Vueling.Checkin.Library.UnitTest", "Vueling.DeferredCheckin.Publisher.Library", "Vueling.DeferredCheckin.Publisher.Library.UnitTest", "Vueling.DeferredCheckin.Infrastructure" } },
                {"AssembliesToProcess", new List<string> {"Vueling"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_SkySales_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\SkySales\INT-20150107",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "SkySales", "SkySales.IntegrationTest", "SkySales.ProxiesServices.ServiceLibrary", "SkySales.UnitTest" } },
                {"AssembliesToProcess", new List<string> {"Vueling", "SkySales"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_SkySales_PRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\SkySales\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "SkySales", "SkySales.IntegrationTest", "SkySales.ProxiesServices.ServiceLibrary", "SkySales.UnitTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingRazor_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Razor\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Razor.ClassLibrary", "Vueling.Razor.ConsoleApplicationTest", "Vueling.Razor.Contracts.ServiceLibrary", "Vueling.Razor.Impl.ServiceLibrary", "Vueling.Razor.Contracts.ServiceLibrary.UnitTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingCache_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Cache\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Cache.AppFabric.Impl.ServiceLibrary.IntegrationTest", "Vueling.Cache.AppFabric.Infrastructure", "Vueling.Cache.Client.ServiceLibrary", "Vueling.Cache.Common.Library", "Vueling.Cache.DataKey.ServiceLibrary", "Vueling.Cache.DataKey.ServiceLibrary.UnitTest", "Vueling.Cache.DatesTasks.ServiceLibrary", "Vueling.Cache.Maestros.ServiceLibrary", "Vueling.Cache.Maestros.WCF.WebService", "Vueling.Cache.Modeling", "Vueling.Cache.Navitaire.Contracts.ServiceLibrary", "Vueling.Cache.Navitaire.Impl.ServiceLibrary", "Vueling.Cache.Navitaire.Impl.ServiceLibrary.IntegrationTest", "Vueling.Cache.Navitaire.Infrastructure", "Vueling.Cache.Proxy.Contracts.ServiceLibrary", "Vueling.Cache.Proxy.ServiceLibrary", "Vueling.Cache.Proxy.ServiceLibrary.IntegrationTest", "Vueling.Cache.Queries.Contracts.ServiceLibrary", "Vueling.Cache.Queries.Impl.ServiceLibrary", "Vueling.Cache.Update.WCF.WebServiceVueling.Cache.Ventus.Contracts.ServiceLibrary", "Vueling.Cache.Ventus.Impl.ServiceLibrary", "Vueling.Cache.WCF.AppFabric.WebService", "Vueling.Cache.WCF.Generation.WebService", "Vueling.Cache.WCF.Generation.WebService.IntegrationTest", "Vueling.Cache.WCF.Navitaire.WebService", "Vueling.Cache.WCF.Navitaire.WebService.IntegrationTest", "Vueling.Cache.WCF.Navitaire.WebService.UnitTest", "Vueling.Cache.WCF.REST.WebService", "Vueling.Cache.WCF.REST.WebService.IntegrationTest", "Vueling.Cache.WCF.REST.WebService.UnitTest", "Vueling.Cache.WCF.Update.WebService", "Vueling.Cache.WCF.WebService", "Vueling.Cache.WebUI", "Vueling.Cache.WebUI.UnitTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingCache_PRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\Vueling.Cache\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\Vueling\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Cache.AppFabric.Impl.ServiceLibrary.IntegrationTest", "Vueling.Cache.AppFabric.Infrastructure", "Vueling.Cache.Client.ServiceLibrary", "Vueling.Cache.Common.Library", "Vueling.Cache.DataKey.ServiceLibrary", "Vueling.Cache.DataKey.ServiceLibrary.UnitTest", "Vueling.Cache.DatesTasks.ServiceLibrary", "Vueling.Cache.Maestros.ServiceLibrary", "Vueling.Cache.Maestros.WCF.WebService", "Vueling.Cache.Modeling", "Vueling.Cache.Navitaire.Contracts.ServiceLibrary", "Vueling.Cache.Navitaire.Impl.ServiceLibrary", "Vueling.Cache.Navitaire.Impl.ServiceLibrary.IntegrationTest", "Vueling.Cache.Navitaire.Infrastructure", "Vueling.Cache.Proxy.Contracts.ServiceLibrary", "Vueling.Cache.Proxy.ServiceLibrary", "Vueling.Cache.Proxy.ServiceLibrary.IntegrationTest", "Vueling.Cache.Queries.Contracts.ServiceLibrary", "Vueling.Cache.Queries.Impl.ServiceLibrary", "Vueling.Cache.Update.WCF.WebServiceVueling.Cache.Ventus.Contracts.ServiceLibrary", "Vueling.Cache.Ventus.Impl.ServiceLibrary", "Vueling.Cache.WCF.AppFabric.WebService", "Vueling.Cache.WCF.Generation.WebService", "Vueling.Cache.WCF.Generation.WebService.IntegrationTest", "Vueling.Cache.WCF.Navitaire.WebService", "Vueling.Cache.WCF.Navitaire.WebService.IntegrationTest", "Vueling.Cache.WCF.Navitaire.WebService.UnitTest", "Vueling.Cache.WCF.REST.WebService", "Vueling.Cache.WCF.REST.WebService.IntegrationTest", "Vueling.Cache.WCF.REST.WebService.UnitTest", "Vueling.Cache.WCF.Update.WebService", "Vueling.Cache.WCF.WebService", "Vueling.Cache.WebUI", "Vueling.Cache.WebUI.UnitTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingMyVuelingCity_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.MyVuelingCity\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.MyVuelingCity.Contracts.Infrastructure", "Vueling.MyVuelingCity.Contracts.ServiceLibrary", "Vueling.MyVuelingCity.Impl.Infrastructure", "Vueling.MyVuelingCity.Impl.ServiceLibrary", "Vueling.MyVuelingCity.Impl.ServiceLibrary.UnitTest", "Vueling.MyVuelingCity.Impl.SL.IntegrationTest", "Vueling.MyVuelingCity.Infrastructure.IntegrationTest", "Vueling.MyVuelingCity.WebUI" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_NotificationMail_DEV()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.NotificationMail\DEV",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.NotificationMail.Inventory.Contract.ServiceLibrary", "Vueling.NotificationMail.Inventory.DB.Infrastructure", "Vueling.NotificationMail.Inventory.Impl.ServiceLibrary", "Vueling.NotificationMail.Inventory.Impl.ServiceLibrary.UnitTest", "Vueling.NotificationMail.Inventory.WCF.WebService", "Vueling.NotificationMail.Library", "Vueling.NotificationMail.Library.UnitTest", "Vueling.NotificationMail.QueueMessageRequest.Library", "Vueling.NotificationMail.Reporter.Contracts.ServiceLibrary", "Vueling.NotificationMail.Reporter.Impl.ServiceLibrary", "Vueling.NotificationMail.Reporter.Impl.ServiceLibrary.UnitTest", "Vueling.NotificationMail.Reporter.WindowsService", "Vueling.NotificationMail.ServiceLibrary", "Vueling.NotificationMail.ServiceLibrary.UnitTest", "Vueling.NotificationMail.WCF.WindowsService", "Vueling.NotificationMail.WCF.WindowsService.IntegrationTest", "Vueling.NotificationMail.WCF.WindowsService.UnitTest", "Vueling.NotificationMail.WebCheckin.Contracts.ServiceLibrary", "Vueling.NotificationMail.WebCheckin.Impl.ServiceLibrary", "Vueling.NotificationMail.WebCheckin.Impl.ServiceLibrary.UnitTest", "Vueling.NotificationMail.WebCheckin.WCF.WindowsService", "Vueling.NotificationMail.WebCheckin.WCF.WindowsService.IntegrationTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingSeats_DEV()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Seats\DEV",
                Environment = "DEV",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Seats.Contracts.ServiceLibrary", "Vueling.Seats.Duo.Library.Infrastructure", "Vueling.Seats.Duo.ServiceLibrary", "Vueling.Seats.Duo.ServiceLibrary.IntegrationTest", "Vueling.Seats.Duo.WCF.WebService", "Vueling.Seats.Duo.WCF.WindowsService", "Vueling.Seats.Grupos.WCF.WebService", "Vueling.Seats.Impl.ServiceLibrary", "Vueling.Seats.Library", "Vueling.Seats.Logic.ServiceLibrary", "Vueling.Seats.Logic.ServiceLibrary.IntegrationTest", "Vueling.Seats.Logic.ServiceLibrary.UnitTest", "Vueling.Seats.Logic.WCF.WebService", "Vueling.Seats.MasterLogic.ServiceLibrary", "Vueling.Seats.ResiberShared.Contracts.ServiceLibrary", "Vueling.Seats.ResiberShared.Impl.ServiceLibrary", "Vueling.Seats.ServiceLibrary", "Vueling.Seats.ServiceLibrary.IntegrationTest", "Vueling.Seats.ServiceLibrary.UnitTest", "Vueling.Seats.Shared.Contracts.ServiceLibrary", "Vueling.Seats.Shared.Impl.ServiceLibrary", "Vueling.Seats.WebUI" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingBoardingPass_DEV()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.BoardingPass\DEV",
                Environment = "DEV",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.BoardingPass.DB.Infrastructure", "Vueling.BoardingPass.ErrorLogging.WCF.WebService", "Vueling.BoardingPass.ErrorLogging.WCF.WebService.IntegrationTest", "Vueling.BoardingPass.Library", "Vueling.BoardingPass.Request.WCF.WebService", "Vueling.BoardingPass.Request.WCF.WebService.IntegrationTest", "Vueling.BoardingPass.ServiceLibrary", "Vueling.BoardingPass.ServiceLibrary.IntegrationTest", "Vueling.BoardingPass.ServiceLibrary.UnitTest", "Vueling.BoardingPass.SmsCheck.WCF.WebService", "Vueling.BoardingPass.SmsCheck.WCF.WebService.IntegrationTest", "Vueling.BoardingPass.WebUI", "Vueling.BoardingPass.WebUI.IntegrationTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingReminderEmail_DEV()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.ReminderEmail\DEV",
                Environment = "DEV",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingReminderEmail_PRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\Vueling.ReminderEmail\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\Vueling\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingConfigurationConfig_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Configuration\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Configuration.Config", "Vueling.Configuration.Library", "Vueling.Configuration.Library.UnitTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingRemo_PRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"D:\workspaces\tfs\Vueling.Remo\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"D:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Remo.ServiceLibrary", "Vueling.RemoServiceLibrary.UnitTest", "Vueling.Remo.WCF.WebService", "Vueling.Remo.WindowsUI", "Vueling.Remo.WindowsUI.Installer" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingMaestros_PRE()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling.Maestros\PRE",
                Environment = "PRE",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\BuildProcessTemplate\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Maestros.BO.Common.Library", "Vueling.Maestros.BO.Infrastructure", "Vueling.Maestros.BO.Infrastructure", "Vueling.Maestros.BO.ServiceLibrary.IntegrationTest", "Vueling.Maestros.BO.ServiceLibrary.UnitTest", "Vueling.Maestros.Cache.Infrastructure", "Vueling.Maestros.CMS.Contracts.ServiceLibrary", "Vueling.Maestros.CMS.Impl.ServiceLibrary", "Vueling.Maestros.CMS.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Contracts.ServiceLibrary", "Vueling.Maestros.Impl.ServiceLibrary", "Vueling.Maestros.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Queries.Infrastructure", "Vueling.Maestros.Schedule.WCF.WebService", "Vueling.Maestros.ServiceLibrary.IntegrationTest", "Vueling.Maestros.ServiceLibrary.UnitTest", "Vueling.Maestros.Setting.FootNote.Impl.SL.IntegrationTest", "Vueling.Maestros.Settings.BO.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.BO.DB.Infrastructure", "Vueling.Maestros.Settings.BO.DB.Infrastructure.IntegrationTest", "Vueling.Maestros.Settings.BO.Impl.ServiceLibrary", "Vueling.Maestros.Settings.BO.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.Change.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.Change.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Change.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Settings.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.DB.Infrastructure", "Vueling.Maestros.Settings.DB.Infrastructure.IntegrationTest", "Vueling.Maestros.Settings.Fare.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.Fare.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Fare.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Settings.Fare.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.Fee.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.Fee.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Fee.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Settings.Fee.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.FootNote.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.FootNote.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Library", "Vueling.Maestros.Settings.Payment.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.Payments.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.Payments.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Payments.Impl.SL.IntegrationTest", "Vueling.Maestros.Settings.Role.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.Role.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Role.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Settings.Role.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.Segment.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.Segment.Impl.ServiceLibrary", "Vueling.Maestros.Settings.Segment.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Settings.Segment.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.SSRs.Contracts.ServiceLibrary", "Vueling.Maestros.Settings.SSRs.Impl.ServiceLibrary", "Vueling.Maestros.Settings.SSRs.Impl.ServiceLibrary.IntegrationTest", "Vueling.Maestros.Settings.SSRs.Impl.ServiceLibrary.UnitTest", "Vueling.Maestros.Settings.WCF.WebService", "Vueling.Maestros.WCF.REST.WebService", "Vueling.Maestros.WCF.REST.WebService.IntegrationTest", "Vueling.Maestros.WCF.WebService", "Vueling.Maestros.WebUI", "Vueling_Maestros.Database" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }

        [TestMethod]
        public void Valid_VuelingBuild_INT()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyVersionValidation
            {
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\Vueling.Build\INT",
                Environment = "INT",
                FullPathLibAssemblyExceptions = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\LibAssemblyExceptions.xml",
                FullPathFilteredAssemblies = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\NuGet\FilteredLibraries.json"
            };

            var parameters = new Dictionary<string, object>
            {
                { "ProjectList", new List<string> { "Vueling.Build.Config.ConsoleUI","Vueling.Build.CodeCoverage.ConsoleUI","Vueling.Build.FTPSync.ConsoleUI","Vueling.Build.Scripts.ConsoleUI","Vueling.Build.Scripts.AutoSetupEnvironment.ConsoleUI","Vueling.Build.Scripts.ValidateEnvironment.ConsoleUI","Vueling.Build.Configuration.Global.Bundle.ConsoleUI","Vueling.Build.Navitaire.Web.Deploy.Manager.ConsoleUI","Vueling.Build.Javascript.Minifier.BundleFiles.ConsoleUI","Vueling.Build.StaticClass.Library","Vueling.Build.Javascript.Minifier.ConsoleUI","Vueling.Build.Contracts.ServiceLibrar","Vueling.Build.Contracts.ServiceLibrary.UnitTest","Vueling_Build.Database","Vueling.Build.DB.Infrastructure","Vueling.Build.Impl.ServiceLibrary","Vueling.Build.Impl.ServiceLibrary.IntegrationTest","Vueling.Build.Deploy.Config.Contracts.ServiceLibrary","Vueling.Build.Deploy.Config.Impl.ServiceLibrary","Vueling.Build.Deploy.Proxy.Contracts.ServiceLibrary","Vueling.Build.Deploy.Proxy.Impl.ServiceLibrary","Vueling.Build.Deploy.Config.DTO","Vueling.Build.Deploy.AWS.Contracts.ServiceLibrary","Vueling.Build.Deploy.AzurePlataform.Contracts.ServiceLibrary","Vueling.Build.Deploy.AWS.Impl.ServiceLibrary","Vueling.Build.Deploy.AzurePlataform.Impl.ServiceLibrary","Vueling.Build.Deploy.AWS.IntegrationTest" } },
                {"AssembliesToProcess", new List<string> {"*"}},
            };

            var invoker = new WorkflowInvoker(target);

            var actual = invoker.Invoke(parameters);

            Assert.AreEqual(true, actual["AreVersionValid"]);
        }
    }
}