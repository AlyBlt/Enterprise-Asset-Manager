using MediatR;


namespace AssetManager.Application.Features.Asset.Commands.AssignAsset
{
    public record AssignAssetCommand(int AssetId, int UserId) : IRequest<bool>;
}
