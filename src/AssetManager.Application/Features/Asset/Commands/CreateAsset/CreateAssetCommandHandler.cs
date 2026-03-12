using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AssetManager.Core.Enums;
using AutoMapper;
using MediatR;

namespace AssetManager.Application.Features.Asset.Commands.CreateAsset
{
    public class CreateAssetCommandHandler(
    IAssetRepository assetRepository,
    IMapper mapper,
    IAuditLogService auditLogService) : IRequestHandler<CreateAssetCommand, bool>
    {
        public async Task<bool> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
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
    }
}
