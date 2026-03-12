using AssetManager.Application.Features.Dashboard.Queries.GetDashboardSummary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[Authorize] // Genelde dashboard verileri giriş yapmış kullanıcılara özeldir
public class DashboardController: BaseController 
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await Mediator.Send(new GetDashboardSummaryQuery());
        return Ok(result);
    }
}