using AssetManager.Application.Features.Auth.Commands.Login;
using AssetManager.Application.Features.Auth.Commands.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[AllowAnonymous] // Auth işlemleri genellikle herkese açıktır
public class AuthController : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        // ValidationBehavior hatalı girişi zaten yakalayıp Exception fırlatacaktır.
        var result = await Mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }
}