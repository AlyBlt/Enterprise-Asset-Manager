using AssetManager.Application.DTOs.Asset;

namespace AssetManager.Web.Interfaces;

public interface IAssetApiService
{
    Task<IEnumerable<AssetResponseDto>> GetAllAsync();
    Task<AssetResponseDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateAssetRequestDto request);
    Task<bool> DeleteAsync(int id);
}