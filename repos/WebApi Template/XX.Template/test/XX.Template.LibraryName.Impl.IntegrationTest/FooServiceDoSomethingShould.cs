using System.Net;
using System.Threading.Tasks;
using XX.Template.LibraryName.Impl.IntegrationTest.Fixture;
using Xunit;

namespace XX.Template.LibraryName.Impl.IntegrationTest
{
    public class FooServiceDoSomethingShould : IClassFixture<FooServiceFixture>
    {
        public FooServiceDoSomethingShould(FooServiceFixture fixture)
        {
            // Arrange
            _fixture = fixture;
        }

        private readonly FooServiceFixture _fixture;

        [Theory]
        [Trait("Category", "IntegrationTest")]
        [InlineData("XXXXXX")]
        [InlineData("YYYYYY")]
        public async Task ReturnsAInvalidResponse(string recordLocator)
        {
            // Act
            var response = await _fixture.HttpClient.GetAsync($"/v1/MyService/RecordLocator/{recordLocator}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task ReturnsAValidResponse()
        {
            // Act
            var response = await _fixture.HttpClient.GetAsync("/v1/MyService/RecordLocator/X7CERP");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}