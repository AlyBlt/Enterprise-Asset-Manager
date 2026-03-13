using AssetManager.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace AssetManager.IntegrationTests.System
{
    public class InfoEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public InfoEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetInfo_ShouldReturnApplicationInfo()
        {
            // Act
            var response = await _client.GetAsync("/api/info");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<InfoResponse>();

            result.ShouldNotBeNull();
            result!.Student.ShouldNotBeNullOrEmpty();
            result.Environment.ShouldNotBeNullOrEmpty();
            DateTime.Parse(result.ServerTimeUtc).ShouldBeOfType<DateTime>();
        }
    }
}
