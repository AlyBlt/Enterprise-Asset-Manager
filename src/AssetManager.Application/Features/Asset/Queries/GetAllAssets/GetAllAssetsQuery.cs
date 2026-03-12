using AssetManager.Application.DTOs.Asset;
using MediatR;

namespace AssetManager.Application.Features.Asset.Queries.GetAllAssets
{
    public record GetAllAssetsQuery() : IRequest<IEnumerable<AssetResponseDto>>;
}
