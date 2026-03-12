using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Commands.UnassignAsset
{
    public record UnassignAssetCommand(int AssetId) : IRequest<bool>;
}
