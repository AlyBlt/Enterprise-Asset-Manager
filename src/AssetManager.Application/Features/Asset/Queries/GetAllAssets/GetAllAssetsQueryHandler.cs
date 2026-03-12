using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AssetManager.Application.Features.Asset.Queries.GetAllAssets
{
    public class GetAllAssetsQueryHandler(
    IAssetRepository assetRepository,
    IMapper mapper) : IRequestHandler<GetAllAssetsQuery, IEnumerable<AssetResponseDto>>
    {
        public async Task<IEnumerable<AssetResponseDto>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
        {
            var assets = await assetRepository.GetAllWithUserAsync();
            return mapper.Map<IEnumerable<AssetResponseDto>>(assets);
        }
    }
}
