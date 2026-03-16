using MediatR;

namespace AssetManager.Application.Features.Asset.Commands.UpdateAsset;

public record UpdateAssetCommand(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    string SerialNumber,
    int? AssignedUserId // Update sayfasında atama değişikliği için
) : IRequest<bool>;