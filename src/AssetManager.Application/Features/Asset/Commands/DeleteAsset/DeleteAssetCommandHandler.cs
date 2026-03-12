using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Commands.DeleteAsset
{
    public class DeleteAssetCommandHandler(
    IAssetRepository assetRepository,
    IAuditLogService auditLogService) : IRequestHandler<DeleteAssetCommand, bool>
    {
        public async Task<bool> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await assetRepository.GetByIdAsync(request.Id);

            if (asset == null) return false;

            assetRepository.Delete(asset);
            var result = await assetRepository.SaveChangesAsync() > 0;

            if (result)
            {
                await auditLogService.LogAsync(
                    "Delete",
                    "Asset",
                    request.Id.ToString(),
                    $"{asset.Name} (SN: {asset.SerialNumber}) asset is archived."
                );
            }

            return result;
        }
    }
}
