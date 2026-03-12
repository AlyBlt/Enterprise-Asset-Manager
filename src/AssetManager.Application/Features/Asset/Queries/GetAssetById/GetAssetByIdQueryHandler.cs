using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;


namespace AssetManager.Application.Features.Asset.Queries.GetAssetById
{
    public class GetAssetByIdQueryHandler(
    IAssetRepository assetRepository,
    IMapper mapper) : IRequestHandler<GetAssetByIdQuery, AssetResponseDto?>
    {
        public async Task<AssetResponseDto?> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
        {
            // Repository üzerinden veriyi çekiyoruz
            var asset = await assetRepository.GetWithUserByIdAsync(request.Id);
            if (asset == null) return null;

            // DTO'ya map'leyip dönüyoruz
            return mapper.Map<AssetResponseDto>(asset);
        }
    }
}
