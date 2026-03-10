using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

public class HomeController(IHomeApiService homeApiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        // 1. API'den sistem bilgilerini (öðrenci adý vb.) al
        var viewModel = await homeApiService.GetSystemInfoAsync() ?? new SystemInfoViewModel();

        // 2. API online mý kontrol et ve modele ekle
        viewModel.IsApiOnline = await homeApiService.IsApiHealthyAsync();

        // 3. Veriyi Dashboard (Index) sayfasýna gönder
        return View(viewModel);
    }
}