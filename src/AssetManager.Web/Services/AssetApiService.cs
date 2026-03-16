using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Features.Asset.Commands.UpdateAsset;
using AssetManager.Web.Interfaces;

namespace AssetManager.Web.Services;

public class AssetApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    : BaseApiService(httpClient, httpContextAccessor), IAssetApiService
{
    public async Task<IEnumerable<AssetResponseDto>> GetAllAsync()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync("api/assets");

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<IEnumerable<AssetResponseDto>>() ?? [];

        return [];
    }

    public async Task<AssetResponseDto?> GetByIdAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync($"api/assets/{id}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<AssetResponseDto>()
            : null;
    }

    public async Task<bool> CreateAsync(CreateAssetCommand request)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync("api/assets", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"api/assets/{id}");
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> AssignAsync(int assetId, int userId)
    {
        AddAuthorizationHeader();
        // API Route: [HttpPost("{id}/assign/{userId}")]
        var response = await _httpClient.PostAsync($"api/assets/{assetId}/assign/{userId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UnassignAsync(int id)
    {
        AddAuthorizationHeader();
        // API Route: [HttpPost("{id}/unassign")]
        var response = await _httpClient.PostAsync($"api/assets/{id}/unassign", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(UpdateAssetCommand request)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync($"api/assets/{request.Id}", request);
        return response.IsSuccessStatusCode;
    }
}