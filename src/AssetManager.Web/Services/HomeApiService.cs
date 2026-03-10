using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Home;

namespace AssetManager.Web.Services;

public class HomeApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    : BaseApiService(httpClient, httpContextAccessor), IHomeApiService
{
    public async Task<SystemInfoViewModel?> GetSystemInfoAsync()
    {
        AddAuthorizationHeader();
        // API'deki endpoint: GET /api/info
        var response = await _httpClient.GetAsync("api/info");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SystemInfoViewModel>();
        }

        return null;
    }

    public async Task<bool> IsApiHealthyAsync()
    {
        try
        {
            // API'deki GET /health endpoint'ine gidiyoruz
            var response = await _httpClient.GetAsync("/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            // API kapalıysa veya ulaşılamıyorsa direkt false dön
            return false;
        }
    }
}

