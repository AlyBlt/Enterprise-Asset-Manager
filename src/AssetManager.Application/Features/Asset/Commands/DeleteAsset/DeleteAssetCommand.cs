using MediatR;


namespace AssetManager.Application.Features.Asset.Commands.DeleteAsset
{
    public record DeleteAssetCommand(int Id) : IRequest<bool>;
}
