using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Compression;
using Vueling.Activities.Impl.ServiceLibrary.Compression;

namespace Vueling.Activities.Impl.ServiceLibrary.IntegrationTest.Given_Compression
{
    [TestClass]
    public class When_ZipFiles
    {
        [TestInitialize]
        public void Initialize()
        {
        }
        [TestMethod]
        public void Then_Compress_and_generate_zip()
        {
            IZipService zipService = new ZipService();
            zipService.Initialize(Environment.CurrentDirectory + "\\CompressedFile\\compressed.zip",
                new List<string>()
                {
                    Environment.CurrentDirectory + @"\fake\css",
                    Environment.CurrentDirectory + @"\fake\xml\Views\AgencyContact.xml",
                },
                new List<string>()
                {
                    /*"package.json",
                    "Views"*/
                });
            zipService.InternalExecute();
        }

        [TestMethod]
        public void Then_Compress_and_generate_from_wildcards()
        {
            IZipService zipService = new ZipService();
            zipService.Initialize(Environment.CurrentDirectory + "\\CompressedFile\\compressed.zip",
                new List<string>()
                {
                    Environment.CurrentDirectory + @"\fake\*",
                },
                new List<string>()
                {
                    /*"package.json",
                    "Views"*/
                });
            zipService.InternalExecute();
        }
    }
}
