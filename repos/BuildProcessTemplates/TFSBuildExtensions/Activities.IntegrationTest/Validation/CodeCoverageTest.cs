using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Activities;
using Microsoft.TeamFoundation.Build.Client;
using NMock;

namespace Activities.UnitTest.Validation
{
    [TestClass]
    public class CodeCoverageTest
    {
        #region private properties
        
        /*
        private string _sourcesDirectory = @"D:\workspaces\tfs\";
        private string _binariesDirectory = @"D:\workspaces\tfs\Vueling.XXX\PRE\Vueling.XXX.ServiceLibrary\bin\Debug";
        private string _testRootPath = @"D:\workspaces\tfs\Vueling.XXX\PRE\TestResults";
        */

        private string _sourcesDirectory = @"D:\Temp\Vueling.XXX.PRE.64\Sources";
        private string _binariesDirectory = @"D:\Temp\Vueling.XXX.PRE.64\Binaries";
        private string _testRootPath = @"D:\Temp\Vueling.XXX.PRE.64\TestResults";

        private static MockFactory _MockFactory { get; set; }
        private static Mock<IBuildDetail> _buildDetail { get; set; }
        private static Mock<IBuildDefinition> _buildDefinition { get; set; }

        #endregion

        [TestInitialize]
        public void Initizalize()
        {
            try
            {
                _MockFactory = new MockFactory();
                _buildDetail = _MockFactory.CreateMock<IBuildDetail>();
                _buildDefinition = _MockFactory.CreateMock<IBuildDefinition>();

                _buildDetail.IgnoreUnexpectedInvocations = true;

                _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
                _buildDetail.Expects.AtLeastOne.Method(x => x.Save());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void ShouldFailWhenCodeCoverageInstalledSkySales()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = @"D:\workspaces\tfs\SKYSALES\PRE\SkySales\bin\Debug",
                TestRootPath = @"D:\Temp\TestResults",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void ShouldNotFailWhenCodeCoverageInstalledSkySales()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = @"D:\workspaces\tfs\SKYSALES\PRE\SkySales\bin\Debug",
                TestRootPath = @"D:\Temp\TestResults",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);            
        }

        [TestMethod]
        public void ShouldNotFailWhenCodeCoverageInstalled()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
            };

            _buildDetail.Expects.AtLeastOne.GetProperty<string>(x => x.BuildNumber).WillReturn("Vueling.XXX.INT.64_20130612.4");
            _buildDetail.Expects.AtLeastOne.GetProperty<IBuildDefinition>(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty<string>(x => x.Name).WillReturn("Vueling.XXX.INT.64");

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);
            invoker.Extensions.Add(_buildDefinition.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void KoFailAlthoughBuildCodeCoverageErrorThreshold()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                BuildCodeCoveragePercentageError = 30
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void KoFailBuildCodeCoverageErrorThreshold()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                BuildCodeCoveragePercentageError = 40
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.BuildCodeCoverageError, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void KoFailAssemblyCodeCoverageErrorThreshold()
        {
            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());

            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                AssemblyCodeCoveragePercentageError = 90
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.AssemblyCodeCoverageError, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void OkFailBuildCodeCoverageErrorThreshold()
        {
            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());

            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                BuildCodeCoveragePercentageError = 20
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void OkFailAssemblyCodeCoverageErrorThreshold()
        {
            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());

            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                AssemblyCodeCoveragePercentageError = Decimal.Parse("0,002")
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void OkBuildCodeCoverageWarningThreshold()
        {
            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());

            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                AssemblyCodeCoveragePercentageWarning = Decimal.Parse("0,002")
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void KoBuildCodeCoverageWarningThreshold()
        {
            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());

            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                BuildCodeCoveragePercentageWarning = 50
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.BuildCodeCoverageWarning, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void KoAssemblyCodeCoverageWarningThreshold()
        {
            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());

            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
                AssemblyCodeCoveragePercentageWarning = 10
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.AssemblyCodeCoverageWarning, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void ShouldFailWhenCodeCoverageConsoleIsNotInstalled()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = @"C:\wrong\directory",
                BinariesDirectory = _binariesDirectory,
                TestRootPath = _testRootPath,
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.CodeCoverageConsoleNotFound, output["ResponseCodeCoverage"]);
        }

        [TestMethod]
        public void ShouldFailWhenTestResultsNotFound()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = _sourcesDirectory,
                BinariesDirectory = _binariesDirectory,
                TestRootPath = @"C:\folder\not\found",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.DataCoverageFileNotFound, output["ResponseCodeCoverage"]);
        }
        [TestMethod]
        public void ShouldNotFailAllocationDEV()
        {
            var target = new TFSBuildExtensions.CodeCoverage.CodeCoverageActivity
            {
                SourcesDirectory = @"D:\Temp\Vueling.Allocation.DEV.64\Sources",
                BinariesDirectory = @"D:\Temp\Vueling.Allocation.DEV.64\Binaries",
                TestRootPath = @"D:\Temp\Vueling.Allocation.DEV.64\TestResults",
            };

            _buildDetail.Expects.AtLeastOne.GetProperty<string>(x => x.BuildNumber).WillReturn("Vueling.Allocation.DEV.64_20131114.8");
            _buildDetail.Expects.AtLeastOne.GetProperty<IBuildDefinition>(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty<string>(x => x.Name).WillReturn("Vueling.Allocation.DEV.64");

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDetail.MockObject);
            invoker.Extensions.Add(_buildDefinition.MockObject);

            var output = invoker.Invoke();

            Assert.AreEqual(TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok, output["ResponseCodeCoverage"]);
        }
    }
}
