using AssetManager.Application.DTOs.Asset;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

[Authorize]
public class AssetsController(IAssetApiService assetApiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var assetsDto = await assetApiService.GetAllAsync();

        var viewModel = new AssetListViewModel
        {
            Assets = assetsDto,
            PageTitle = "Corporate Asset Inventory" 
        };

        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new AssetCreateViewModel());

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(AssetCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var requestDto = new CreateAssetRequestDto
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            Category = model.Category,
            SerialNumber = model.SerialNumber
        };

        var result = await assetApiService.CreateAsync(requestDto);

        if (result) return RedirectToAction(nameof(Index));

        // English Error Message
        ModelState.AddModelError("", "An error occurred while creating the asset.");
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await assetApiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var asset = await assetApiService.GetByIdAsync(id);
        if (asset == null) return NotFound();

        return View(asset);
    }
}