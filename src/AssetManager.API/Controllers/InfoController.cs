using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AssetManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public IActionResult GetInfo()
    {
        var info = new
        {
            // Bu değerler Docker/Github Actions tarafında ayarlanacak
            student = configuration["STUDENT_NAME"] ?? "Name is not configured.",
            environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production",
            serverTimeUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };

        return Ok(info);
    }
}