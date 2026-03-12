using MediatR;

namespace AssetManager.Application.Features.Asset.Commands.CreateAsset
{
    public record CreateAssetCommand(
    string Name,
    string Description,
    decimal Price,
    string Category,
    string SerialNumber) : IRequest<bool>;
}
