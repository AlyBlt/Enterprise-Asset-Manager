using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await authService.RegisterAsync(request);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await authService.LoginAsync(request);
        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }
}