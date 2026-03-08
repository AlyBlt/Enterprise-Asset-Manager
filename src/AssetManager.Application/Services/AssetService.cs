using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AssetManager.Core.Enums;
using AutoMapper;
using FluentValidation; // Validator için eklendi

namespace AssetManager.Application.Services;

public class AssetService(
    IAssetRepository assetRepository,
    IMapper mapper,
    IAuditLogService auditLogService,
    IValidator<CreateAssetRequestDto> createAssetValidator) : IAssetService // Validator eklendi
{
    public async Task<IEnumerable<AssetResponseDto>> GetAllAssetsAsync()
    {
        var assets = await assetRepository.GetAllWithUserAsync();
        return mapper.Map<IEnumerable<AssetResponseDto>>(assets);
    }

    public async Task<AssetResponseDto?> GetAssetByIdAsync(int id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset == null) return null;

        return mapper.Map<AssetResponseDto>(asset);
    }

    public async Task<bool> CreateAssetAsync(CreateAssetRequestDto request)
    {
        // --- VALIDASYON KONTROLÜ ---
        var validationResult = await createAssetValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            // Middleware bu hatayı yakalayıp 400 dönecek
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        // ---------------------------

        var newAsset = mapper.Map<AssetEntity>(request);

        newAsset.Status = AssetStatus.InStock;

        await assetRepository.AddAsync(newAsset);
        var result = await assetRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Create",
                "Asset",
                newAsset.SerialNumber,
                $"New asset is added: {newAsset.Name} (Category: {newAsset.Category})"
            );
        }

        return result;
    }

    public async Task<bool> DeleteAssetAsync(int id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset == null) return false;

        assetRepository.Delete(asset);
        var result = await assetRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Delete",
                "Asset",
                id.ToString(),
                $"{asset.Name} (SN: {asset.SerialNumber}) asset is archived."
            );
        }

        return result;
    }

    public async Task<bool> AssignAssetToUserAsync(int assetId, int userId)
    {
        var asset = await assetRepository.GetByIdAsync(assetId);

        if (asset == null || asset.Status != AssetStatus.InStock)
            return false;

        asset.AssignedUserId = userId;
        asset.Status = AssetStatus.Assigned;

        assetRepository.Update(asset);
        var result = await assetRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Assign",
                "Asset",
                assetId.ToString(),
                $"{asset.Name} (SN: {asset.SerialNumber}) is assigned to user: {userId}"
            );
        }

        return result;
    }

    public async Task<bool> UnassignAssetAsync(int assetId)
    {
        var asset = await assetRepository.GetByIdAsync(assetId);

        if (asset == null || asset.AssignedUserId == null)
            return false;

        var previousUserId = asset.AssignedUserId;

        asset.AssignedUserId = null;
        asset.Status = AssetStatus.InStock;

        assetRepository.Update(asset);
        var result = await assetRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Unassign",
                "Asset",
                assetId.ToString(),
                $"{asset.Name} (SN: {asset.SerialNumber}) is returned to stock from user: {previousUserId}"
            );
        }

        return result;
    }
}