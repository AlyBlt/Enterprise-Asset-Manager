using AssetManager.Application.Features.Asset.Commands.AssignAsset;
using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Features.Asset.Commands.DeleteAsset;
using AssetManager.Application.Features.Asset.Commands.UnassignAsset;
using AssetManager.Application.Features.Asset.Queries.GetAllAssets;
using AssetManager.Application.Features.Asset.Queries.GetAssetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[Authorize]
public class AssetsController : BaseController
{
    // Tüm varlıkları listele
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllAssetsQuery());
        return Ok(result);
    }

    // ID'ye göre tek bir varlık getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetAssetByIdQuery(id));
        if (result == null) return NotFound($"ID: {id} asset is not found.");

        return Ok(result);
    }

    // Yeni varlık ekle (Sadece Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAssetCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result) return BadRequest("An error occurred while adding the asset.");

        return Ok(new { message = "Asset is successfully added." });
    }

    // Varlığı sil (Soft Delete - Sadece Admin)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteAssetCommand(id));
        if (!result) return NotFound($"ID: {id} asset is not found and couldn't be deleted!");

        return Ok(new { message = "Asset is successfully deleted (Archived)." });
    }

    // Zimmetle (Sadece Admin)
    [HttpPost("{id}/assign/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(int id, int userId)
    {
        var result = await Mediator.Send(new AssignAssetCommand(id, userId));
        if (!result) return BadRequest("Asset is not found!");

        return Ok(new { message = "Asset is successfully assigned." });
    }

    // Zimmetten Çıkar (Sadece Admin)
    [HttpPost("{id}/unassign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unassign(int id)
    {
        var result = await Mediator.Send(new UnassignAssetCommand(id));
        if (!result) return BadRequest("Asset is not found or not assigned!");

        return Ok(new { message = "Asset is unassigned and now in the stock." });
    }
}