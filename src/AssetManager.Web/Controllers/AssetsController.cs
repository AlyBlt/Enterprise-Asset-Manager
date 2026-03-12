using AssetManager.Application.Features.Asset.Commands.CreateAsset;
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

        var command = new CreateAssetCommand(
        model.Name,
        model.Description,
        model.Price,
        model.Category,
        model.SerialNumber
        );

        var result = await assetApiService.CreateAsync(command);

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