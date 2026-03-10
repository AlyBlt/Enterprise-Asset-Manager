using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

[Authorize]
public class DashboardController(IDashboardApiService dashboardApiService) : Controller
{
    public async Task<IActionResult> Index()
    {
       
        var viewModel = await dashboardApiService.GetDashboardSummaryAsync();

        if (viewModel == null)
        {
            return View(new DashboardViewModel());
        }

        // Güvenlik Kontrolü: Admin değilse logları temizle.
        if (!User.IsInRole("Admin"))
        {
            viewModel.RecentActivities.Clear();
        }

        return View(viewModel);
    }
}