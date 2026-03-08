using AssetManager.Application.DTOs.Asset;
using AssetManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //Bu kontrolcüye erişmek için geçerli bir JWT Token şart!
public class AssetsController(IAssetService assetService) : ControllerBase
{
    // Tüm varlıkları listele (Her kullanıcı görebilir)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetResponseDto>>> GetAll()
    {
        var assets = await assetService.GetAllAssetsAsync();
        return Ok(assets);
    }

    // ID'ye göre tek bir varlık getir
    [HttpGet("{id}")]
    public async Task<ActionResult<AssetResponseDto>> GetById(int id)
    {
        var asset = await assetService.GetAssetByIdAsync(id);
        if (asset == null) return NotFound($"ID: {id} asset is not found.");

        return Ok(asset);
    }

    // Yeni varlık ekle (Sadece Admin yetkisi olanlar)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequestDto request)
    {
        var result = await assetService.CreateAssetAsync(request);
        if (!result) return BadRequest("An error occurred while adding the asset.");

        // Profesyonel yaklaşım: 201 Created dönmek
        return CreatedAtAction(nameof(GetById), new { id = request.SerialNumber }, new { message = "Asset is successfully added." });
    }

    
    // Varlığı sil (Soft Delete - Sadece Admin yetkisi olanlar)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin rolü silebilir
    public async Task<IActionResult> Delete(int id)
    {
        var result = await assetService.DeleteAssetAsync(id);
        if (!result) return NotFound($"ID: {id} asset is not found and couldn't be deleted!");

        return Ok(new { message = "Asset is successfully deleted (Archived)." });
    }

    [HttpPost("{id}/assign/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(int id, int userId)
    {
        var result = await assetService.AssignAssetToUserAsync(id, userId);
        if (!result) return BadRequest("Asset is not found!");

        return Ok(new { message = "Asset is successfully assigned." });
    }

    [HttpPost("{id}/unassign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unassign(int id)
    {
        var result = await assetService.UnassignAssetAsync(id);
        if (!result) return BadRequest("Asset is not found or not assigned!");

        return Ok(new { message = "Asset is unassigned and now in the stock." });
    }
}