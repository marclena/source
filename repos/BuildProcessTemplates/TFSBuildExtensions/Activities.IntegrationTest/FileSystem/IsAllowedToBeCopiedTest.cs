using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.UnitTest.FileSystem
{
    [TestClass]
    public class IsAllowedToBeCopiedTest
    {
        [TestMethod]
        public void FileAllowedToBeCopied()
        {
            var target = new TFSBuildExtensions.FileSystem.IsAllowedToBeCopiedActivity 
            {
                File = @"bin\Newtonsoft.Json.dll",
                WebsiteRoot = @"D:\workspaces\tfs\SkySales\PRE\SkySales",
                ExcludeFilter = @"bin\*,*.config,*orig,runtimehost.naml,ZipSkins\*,thumbs.db,naml\runtimehost.naml",
                FilterExclusions = @"bin\Vueling*,bin\Autofac*,bin\Fasterflect*,bin\log4net*,bin\CommonServices.Operations.*,bin\CommonServices.Schedule.*,bin\Data.DataAccess.*,bin\DiffieHellman.*,bin\Esent.Interop.*,bin\HtmlAgilityPack.*,bin\ICSharpCode.SharpZipLib.*,bin\Newtonsoft.Json.Net35.*,bin\Org.Mentalis.Security.*,bin\SkySales.XmlSerializers.*,bin\Tamir.SharpSSH.*,bin\Wintellect.Threading.*,bin\skysales.*,bin\SkySales.ProxiesServices.ServiceLibrary.*,bin\Microsoft.ApplicationServer.Caching.Client.*,bin\Microsoft.ApplicationServer.Caching.Core.*,bin\Microsoft.WindowsFabric.Common.*,bin\Microsoft.WindowsFabric.Data.Common.*,bin\Config,bin\Config\*",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(true, output["IsAllowed"]);
        }

        [TestMethod]
        public void FileNotAllowedToBeCopied()
        {
            var target = new TFSBuildExtensions.FileSystem.IsAllowedToBeCopiedActivity
            {
                File = @"naml\runtimehost.naml",
                WebsiteRoot = @"D:\workspaces\tfs\SkySales\INT\SkySales",
                ExcludeFilter = @"bin\*,*.config,*orig,runtimehost.naml,ZipSkins\*,thumbs.db,naml\runtimehost.naml",
                FilterExclusions = @"bin\Vueling*,bin\Autofac*,bin\Fasterflect*,bin\log4net*,bin\CommonServices.Operations.*,bin\CommonServices.Schedule.*,bin\Data.DataAccess.*,bin\DiffieHellman.*,bin\Esent.Interop.*,bin\HtmlAgilityPack.*,bin\ICSharpCode.SharpZipLib.*,bin\Newtonsoft.Json.Net35.*,bin\Org.Mentalis.Security.*,bin\SkySales.XmlSerializers.*,bin\Tamir.SharpSSH.*,bin\Wintellect.Threading.*,bin\skysales.*,bin\SkySales.ProxiesServices.ServiceLibrary.*,bin\Microsoft.ApplicationServer.Caching.Client.*,bin\Microsoft.ApplicationServer.Caching.Core.*,bin\Microsoft.WindowsFabric.Common.*,bin\Microsoft.WindowsFabric.Data.Common.*,bin\Config,bin\Config\*",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(false, output["IsAllowed"]);
        }

        [TestMethod]
        public void WebConfigNotAllowedToBeCopied()
        {
            var target = new TFSBuildExtensions.FileSystem.IsAllowedToBeCopiedActivity
            {
                File = @"\web.config",
                WebsiteRoot = @"D:\workspaces\tfs\SkySales\INT\SkySales",
                ExcludeFilter = @"bin\*,*.config,*orig,runtimehost.naml,ZipSkins\*,thumbs.db,naml\runtimehost.naml",
                FilterExclusions = @"bin\Vueling*,bin\Autofac*,bin\Fasterflect*,bin\log4net*,bin\CommonServices.Operations.*,bin\CommonServices.Schedule.*,bin\Data.DataAccess.*,bin\DiffieHellman.*,bin\Esent.Interop.*,bin\HtmlAgilityPack.*,bin\ICSharpCode.SharpZipLib.*,bin\Newtonsoft.Json.Net35.*,bin\Org.Mentalis.Security.*,bin\SkySales.XmlSerializers.*,bin\Tamir.SharpSSH.*,bin\Wintellect.Threading.*,bin\skysales.*,bin\SkySales.ProxiesServices.ServiceLibrary.*,bin\Microsoft.ApplicationServer.Caching.Client.*,bin\Microsoft.ApplicationServer.Caching.Core.*,bin\Microsoft.WindowsFabric.Common.*,bin\Microsoft.WindowsFabric.Data.Common.*,bin\Config,bin\Config\*",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(false, output["IsAllowed"]);

        }
    }
}
