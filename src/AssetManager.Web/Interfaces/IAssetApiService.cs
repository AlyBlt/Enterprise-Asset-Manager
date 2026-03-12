using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;

namespace AssetManager.Web.Interfaces;

public interface IAssetApiService
{
    Task<IEnumerable<AssetResponseDto>> GetAllAsync();
    Task<AssetResponseDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateAssetCommand request);
    Task<bool> DeleteAsync(int id);
}