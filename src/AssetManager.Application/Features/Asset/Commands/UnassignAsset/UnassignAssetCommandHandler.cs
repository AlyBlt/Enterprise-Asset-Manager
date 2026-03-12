using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Commands.UnassignAsset
{
    public class UnassignAssetCommandHandler(
     IAssetRepository assetRepository,
     IAuditLogService auditLogService) : IRequestHandler<UnassignAssetCommand, bool>
    {
        public async Task<bool> Handle(UnassignAssetCommand request, CancellationToken cancellationToken)
        {
            // 1. Asset'i bul
            var asset = await assetRepository.GetByIdAsync(request.AssetId);

            // 2. İş kuralı: Asset var mı ve zaten birine atanmış mı?
            if (asset == null || asset.AssignedUserId == null)
                return false;

            var previousUserId = asset.AssignedUserId;

            // 3. Bilgileri sıfırla ve stok durumuna çek
            asset.AssignedUserId = null;
            asset.Status = AssetStatus.InStock;

            assetRepository.Update(asset);
            var result = await assetRepository.SaveChangesAsync() > 0;

            // 4. Loglama işlemi
            if (result)
            {
                await auditLogService.LogAsync(
                    "Unassign",
                    "Asset",
                    request.AssetId.ToString(),
                    $"{asset.Name} (SN: {asset.SerialNumber}) is returned to stock from user: {previousUserId}"
                );
            }

            return result;
        }
    }
}
