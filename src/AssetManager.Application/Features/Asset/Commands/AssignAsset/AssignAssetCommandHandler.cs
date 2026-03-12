using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Commands.AssignAsset
{
    public class AssignAssetCommandHandler(
    IAssetRepository assetRepository,
    IAuditLogService auditLogService) : IRequestHandler<AssignAssetCommand, bool>
    {
        public async Task<bool> Handle(AssignAssetCommand request, CancellationToken cancellationToken)
        {
            // 1. Asset'i bul
            var asset = await assetRepository.GetByIdAsync(request.AssetId);

            // 2. İş kuralı kontrolü: Asset var mı ve stokta mı?
            if (asset == null || asset.Status != AssetStatus.InStock)
                return false;

            // 3. Atama ve Statü güncelleme
            asset.AssignedUserId = request.UserId;
            asset.Status = AssetStatus.Assigned;

            assetRepository.Update(asset);
            var result = await assetRepository.SaveChangesAsync() > 0;

            // 4. Loglama
            if (result)
            {
                await auditLogService.LogAsync(
                    "Assign",
                    "Asset",
                    request.AssetId.ToString(),
                    $"{asset.Name} (SN: {asset.SerialNumber}) is assigned to user: {request.UserId}"
                );
            }

            return result;
        }
    }
}
