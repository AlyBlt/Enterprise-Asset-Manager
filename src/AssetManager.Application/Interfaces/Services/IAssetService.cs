using AssetManager.Application.DTOs;
using AssetManager.Application.DTOs.Asset;

namespace AssetManager.Application.Interfaces.Services;

public interface IAssetService
{
    Task<IEnumerable<AssetResponseDto>> GetAllAssetsAsync();
    Task<AssetResponseDto?> GetAssetByIdAsync(int id);
    Task<bool> CreateAssetAsync(CreateAssetRequestDto request);
    Task<bool> DeleteAssetAsync(int id);
    Task<bool> AssignAssetToUserAsync(int assetId, int userId);
    Task<bool> UnassignAssetAsync(int assetId);
}