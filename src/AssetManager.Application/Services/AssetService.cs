using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AssetManager.Core.Enums;

namespace AssetManager.Application.Services;

public class AssetService(IGenericRepository<AssetEntity> assetRepository) : IAssetService
{
    public async Task<IEnumerable<AssetResponseDto>> GetAllAssetsAsync()
    {
        // Sadece silinmemiş varlıkları getir (Soft Delete kontrolü)
        var assets = await assetRepository.FindAsync(a => !a.IsDeleted);

        return assets.Select(a => new AssetResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            Price = a.Value, // Value -> Price
            Category = a.Category,
            Status = a.Status.ToString() // Enum'u "Active", "InRepair" gibi stringe çevirir
        });
    }

    public async Task<AssetResponseDto?> GetAssetByIdAsync(int id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset == null) return null;

        return new AssetResponseDto
        {
            Id = asset.Id,
            Name = asset.Name,
            Price = asset.Value,
            Category = asset.Category
        };
    }

    public async Task<bool> CreateAssetAsync(CreateAssetRequestDto request)
    {
        var newAsset = new AssetEntity
        {
            Name = request.Name,
            Description = request.Description,
            Value = request.Price,
            Category = request.Category,
            SerialNumber = request.SerialNumber,
            Status = AssetStatus.Active,
            CreatedAt = DateTime.UtcNow // BaseEntity kuralı
        };

        await assetRepository.AddAsync(newAsset);
        return await assetRepository.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAssetAsync(int id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset == null) return false;

        // Hard delete yerine Soft Delete yapıyoruz
        asset.IsDeleted = true;
        asset.UpdatedAt = DateTime.UtcNow;

        assetRepository.Update(asset);
        return await assetRepository.SaveChangesAsync() > 0;
    }
}