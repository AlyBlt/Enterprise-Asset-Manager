using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Features.Asset.Commands.UpdateAsset;

namespace AssetManager.Web.Interfaces;

public interface IAssetApiService
{
    Task<IEnumerable<AssetResponseDto>> GetAllAsync();
    Task<AssetResponseDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateAssetCommand request);
    Task<bool> DeleteAsync(int id);
    Task<bool> AssignAsync(int assetId, int userId);
    Task<bool> UnassignAsync(int id);
    Task<bool> UpdateAsync(UpdateAssetCommand request);
}