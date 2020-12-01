using System;
using System.Net.Http;
using XX.Template.WebApi;
using Microsoft.AspNetCore.TestHost;

namespace XX.Template.LibraryName.Impl.IntegrationTest.Fixture
{
    public class FooServiceFixture : IDisposable
    {
        public FooServiceFixture()
        {
            // Arrange
            Environment.SetEnvironmentVariable("branchName", "webApi.IntTest");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "test");

            var webHostBuilder = Program.CreateWebHostBuilder(new string[0]);

            HttpClient = new TestServer(webHostBuilder).CreateClient();
        }

        public HttpClient HttpClient { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        ~FooServiceFixture()
        {
            Dispose(false);
        }
    }
}