using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Features.Asset.Commands.UpdateAsset;
using AssetManager.Application.Features.Auth.Commands.Login;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    [Fact]
    public async Task Update_AsEditor_ShouldReturnOk()
    {
        // 1. Arrange
        // Admin olarak bağlan (Asset yaratma yetkisi olduğu için)
        var adminToken = await GetTokenAsync("admin", "admin123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        // Yeni bir asset yarat
        var createCommand = new CreateAssetCommand("Integration Test Asset", "Initial Desc", 100, "IT", "SN-INT-100");
        var createResponse = await _client.PostAsJsonAsync("/api/assets", createCommand);
        createResponse.EnsureSuccessStatusCode(); // Yaratıldığından emin ol

        // ŞİMDİ: Editor olarak bağlan (Güncelleme testi için)
        var editorToken = await GetTokenAsync("jdoe", "editor123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", editorToken);

        // Asset listesini çek ve son eklediğimizin ID'sini al (ID 1 mi değil mi riskine girmiyoruz)
        var assets = await _client.GetFromJsonAsync<List<AssetManager.Application.DTOs.Asset.AssetResponseDto>>("/api/assets");
        var targetAsset = assets.OrderByDescending(a => a.Id).First();

        var updateCommand = new UpdateAssetCommand(targetAsset.Id, "Updated Name", "Updated Desc", 3000m, "IT", "SN-INT-100", null);

        // 2. Act
        var response = await _client.PutAsJsonAsync($"/api/assets/{targetAsset.Id}", updateCommand);

        // 3. Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_AsGuest_ShouldReturnForbidden()
    {
        // 1. Arrange
        // awinehouse -> Guest rolünde bir kullanıcı
        var token = await GetTokenAsync("awinehouse", "user123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        int assetId = 1;
        var command = new UpdateAssetCommand(assetId, "Hack Attempt", "Desc", 1m, "Cat", "SN", null);

        // 2. Act
        var response = await _client.PutAsJsonAsync($"/api/assets/{assetId}", command);

        // 3. Assert
        // Guest kullanıcısının Edit yetkisi olmadığı için 403 dönmeli
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_WithIdMismatch_ShouldReturnBadRequest()
    {
        // 1. Arrange
        var token = await GetTokenAsync("admin", "admin123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        int urlId = 5; // URL'deki ID
        int commandId = 10; // Body'deki ID (Uyuşmazlık!)

        var command = new UpdateAssetCommand(commandId, "Name", "Desc", 100m, "Cat", "SN", null);

        // 2. Act
        var response = await _client.PutAsJsonAsync($"/api/assets/{urlId}", command);

        // 3. Assert
        // Controller'daki 'if (id != command.Id) return BadRequest("ID mismatch!");' kontrolünü test ediyoruz
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}