using BookStore.Service;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace BookStore.IntegrationTests
{
    public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BooksControllerTests(WebApplicationFactory<Program> _factory)
        {
            this._factory = _factory;
        }

        [Fact]
        public async Task GetBooks_ReturnsSuccess()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Books");
            response.EnsureSuccessStatusCode();
        }
    }
}
