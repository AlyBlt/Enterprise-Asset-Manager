using System.Net;
using System.Net.Http.Json;
using AssetManager.Application.Features.Auth.Commands.Login;
using AssetManager.Application.DTOs.Auth;
using Shouldly;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace AssetManager.IntegrationTests.Features.Auth;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        // Factory üzerinden bellekteki API'ye erişecek istemciyi alıyoruz
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidAdminCredentials_ShouldReturnOkAndToken()
    {
        // --- 1. Arrange ---
        // Seed datanda (DatabaseInitializerService) tanımladığın admin bilgileri:
        var command = new LoginCommand("admin", "admin123");

        // --- 2. Act ---
        // Senin AuthController içindeki [HttpPost("login")] endpoint'ine gidiyoruz
        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        // --- 3. Assert ---
        // HTTP Status 200 mü?
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // Dönen sonucu DTO'muza map ediyoruz
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Username.ShouldBe("admin");
        result.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturnUnauthorized()
    {
        // --- 1. Arrange ---
        // Kullanıcı adı doğru ama şifre yanlış senaryosu
        var command = new LoginCommand("admin", "yanlisSifre456");

        // --- 2. Act ---
        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        // --- 3. Assert ---
        // Senin kodundaki 'return Unauthorized(result)' kısmını test ediyoruz
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        result!.IsSuccess.ShouldBeFalse();
        result.Token.ShouldBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ShouldReturnUnauthorized()
    {
        // --- 1. Arrange ---
        var command = new LoginCommand("hayalet_user", "123456");

        // --- 2. Act ---
        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        // --- 3. Assert ---
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}