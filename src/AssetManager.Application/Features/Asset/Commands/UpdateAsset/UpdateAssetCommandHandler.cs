using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssetManager.Application.Features.Asset.Commands.UpdateAsset
{
    public class UpdateAssetCommandHandler(
        IAssetRepository assetRepository,
        IMapper mapper,
        IAuditLogService auditLogService) : IRequestHandler<UpdateAssetCommand, bool>
    {
        public async Task<bool> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            // 1. Mevcut asset'i bul
            var existingAsset = await assetRepository.GetByIdAsync(request.Id);
            if (existingAsset == null) return false;

            // 2. Temel bilgileri map et (Id'yi koruyarak üzerine yazar)
            mapper.Map(request, existingAsset);

            // 3. Atama Mantığını Yönet (Business Logic)
            if (request.AssignedUserId.HasValue)
            {
                // Bir kullanıcı seçilmişse: Atanan kişiyi güncelle ve durumu 'Assigned' yap
                existingAsset.AssignedUserId = request.AssignedUserId.Value;
                existingAsset.Status = AssetStatus.Assigned;
            }
            else
            {
                // Kullanıcı seçilmemişse: Atamayı temizle ve durumu 'Available' (veya InStock) yap
                existingAsset.AssignedUserId = null;
                existingAsset.Status = AssetStatus.InStock;
            }

            // 4. Değişiklikleri Kaydet
            assetRepository.Update(existingAsset);
            var result = await assetRepository.SaveChangesAsync() > 0;

            // 5. Loglama
            if (result)
            {
                await auditLogService.LogAsync(
                    "Update",
                    "Asset",
                    existingAsset.SerialNumber,
                    $"Asset updated: {existingAsset.Name} (Status: {existingAsset.Status})"
                );
            }

            return result;
        }
    }
}