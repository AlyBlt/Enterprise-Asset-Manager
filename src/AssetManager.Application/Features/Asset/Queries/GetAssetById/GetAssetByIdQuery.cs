using AssetManager.Application.DTOs.Asset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Queries.GetAssetById
{
    public record GetAssetByIdQuery(int Id) : IRequest<AssetResponseDto?>;
}
