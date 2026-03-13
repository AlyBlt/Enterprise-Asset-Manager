using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AssetManager.IntegrationTests.System
{
    public class HealthEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public HealthEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetHealth_ShouldReturn200Ok()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
