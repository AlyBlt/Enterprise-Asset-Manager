using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Features.Asset.Commands.UpdateAsset;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetManager.Web.Controllers;

[Authorize]
public class AssetsController(IAssetApiService assetApiService, IAdminApiService adminApiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var assetsDto = await assetApiService.GetAllAsync();

        var users = await adminApiService.GetAllUsersAsync();
        ViewBag.Users = users.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.FullName
        }).ToList();

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

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> Assign(int assetId, int userId)
    {
        var result = await assetApiService.AssignAsync(assetId, userId);
        if (result) TempData["SuccessMessage"] = "Asset assigned successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> Unassign(int assetId)
    {
        var result = await assetApiService.UnassignAsync(assetId);
        if (result) TempData["SuccessMessage"] = "Asset unassigned successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Assets/Edit/5
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> Edit(int id)
    {
        var asset = await assetApiService.GetByIdAsync(id);
        if (asset == null) return NotFound();

        var users = await adminApiService.GetAllUsersAsync();
        ViewBag.UserList = users.Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.FullName
        }).ToList();

        var model = new AssetUpdateViewModel
        {
            Id = asset.Id,
            Name = asset.Name,
            Description = asset.Description,
            Price = asset.Price,
            Category = asset.Category,
            SerialNumber = asset.SerialNumber,
            AssignedUserId = asset.AssignedUserId
        };

        return View(model);
    }

    // POST: Assets/Edit/5
    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> Edit(AssetUpdateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Model geçersizse kullanıcı listesini tekrar doldurup view'a dön
            var users = await adminApiService.GetAllUsersAsync();
            ViewBag.UserList = users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FullName });
            return View(model);
        }

        // ViewModel'den Command'e dönüştür
        var command = new UpdateAssetCommand(
            model.Id,
            model.Name,
            model.Description,
            model.Price,
            model.Category,
            model.SerialNumber,
            model.AssignedUserId
        );

        var result = await assetApiService.UpdateAsync(command);

        if (result)
        {
            TempData["SuccessMessage"] = "Asset updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "An error occurred while updating the asset on the API.");
        return View(model);
    }
}
