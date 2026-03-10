using AssetManager.Application.DTOs.Auth;
using AssetManager.Web.Interfaces;
using System.Net.Http.Json;

namespace AssetManager.Web.Services;

public class AuthApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    : BaseApiService(httpClient, httpContextAccessor), IAuthApiService
{
    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        // Login işleminde henüz token olmadığı için AddAuthorizationHeader çağırmıyoruz
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        }

        return null;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        }

        return null;
    }
}