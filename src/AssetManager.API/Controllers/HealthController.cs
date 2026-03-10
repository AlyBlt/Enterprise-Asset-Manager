using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[ApiController]
[Route("health")] // Ödevde tam olarak GET /health istendiği için api prefix'i koymuyoruz
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(); // 200 OK döner
}