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
        if (asset == null) return NotFound($"{id} numaralı varlık bulunamadı.");

        return Ok(asset);
    }

    // Yeni varlık ekle (Sadece Admin yetkisi olanlar)
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin rolü ekleyebilir
    public async Task<IActionResult> Create([FromBody] CreateAssetRequestDto request)
    {
        var result = await assetService.CreateAssetAsync(request);
        if (!result) return BadRequest("Varlık eklenirken bir hata oluştu.");

        return Ok(new { message = "Varlık başarıyla oluşturuldu." });
    }

    // Varlığı sil (Soft Delete - Sadece Admin yetkisi olanlar)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin rolü silebilir
    public async Task<IActionResult> Delete(int id)
    {
        var result = await assetService.DeleteAssetAsync(id);
        if (!result) return NotFound($"{id} numaralı varlık bulunamadı veya silinemedi.");

        return Ok(new { message = "Varlık başarıyla silindi (Arşivlendi)." });
    }
}