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
        var model = await dashboardApiService.GetDashboardSummaryAsync();

        // Eğer veri gelmezse boş bir model dönerek sayfanın patlamasını engelleriz
        return View(model ?? new DashboardViewModel());
    }
}