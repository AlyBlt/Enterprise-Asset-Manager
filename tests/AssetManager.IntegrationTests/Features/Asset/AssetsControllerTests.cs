using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Features.Auth.Commands.Login;
using Shouldly;
using Xunit;

namespace AssetManager.IntegrationTests.Features.Asset;

public class AssetsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AssetsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // YARDIMCI METOT: Testler için Token alır
    private async Task<string> GetTokenAsync(string username, string password)
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginCommand(username, password));

        // Eğer login olamazsa test burada patlar ve nedenini söyler.
        if (!loginResponse.IsSuccessStatusCode)
        {
            var error = await loginResponse.Content.ReadAsStringAsync();
            throw new Exception($"Login failed for {username}. Status: {loginResponse.StatusCode}, Error: {error}");
        }

        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        return authResult!.Token;
    }

    [Fact]
    public async Task Create_AsAdmin_ShouldReturnOk()
    {
        // 1. Arrange (Hazırlık)
        var token = await GetTokenAsync("admin", "admin123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new CreateAssetCommand(
        "Test Monitor",
        "Integration Test Asset",
        500m,
        "Hardware",
        "TEST-123"
        );

        // 2. Act (Eylem)
        var response = await _client.PostAsJsonAsync("/api/assets", command); 

        // 3. Assert (Doğrulama)
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_AsGuest_ShouldReturnForbidden()
    {
        // 1. Arrange (Hazırlık)
        // Seed datandaki guest kullanıcısı: awinehouse / user123
        var token = await GetTokenAsync("awinehouse", "user123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new CreateAssetCommand(
          "Forbidden Asset",
          "This should fail",
           999m,
          "Security",
          "FORB-001"
         );

        // 2. Act
        // Token'ı her istekte HttpRequestMessage ile manuel ekliyoruz:
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        request.Content = JsonContent.Create(command);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        // 3. Assert
        // Guest kullanıcısının 'Admin' rolü olmadığı için 403 Forbidden dönmeli
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAll_AsAuthorizedUser_ShouldReturnAssetList()
    {
        // 1. Arrange
        var token = await GetTokenAsync("jdoe", "editor123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // 2. Act
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/assets");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        // 3. Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}