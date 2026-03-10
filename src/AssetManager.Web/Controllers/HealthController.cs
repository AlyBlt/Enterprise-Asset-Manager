using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

public class HealthController : Controller
{
    // GET: /health
    // Nginx buraya istek atar, 200 OK alırsa "MVC ayakta" der.
    [HttpGet("/health")]
    public IActionResult Index()
    {
        return Ok();
    }
}