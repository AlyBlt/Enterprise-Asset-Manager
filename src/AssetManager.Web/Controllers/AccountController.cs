using AssetManager.Application.Features.Auth.Commands.Login;
using AssetManager.Application.Features.Auth.Commands.Register;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AssetManager.Web.Controllers;

public class AccountController(IAuthApiService authApiService) : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // ViewModel -> DTO Mapping
        var command = new LoginCommand(
            model.Username,
            model.Password
        );

        var result = await authApiService.LoginAsync(command);

        if (result != null && result.IsSuccess)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Username),
                new Claim(ClaimTypes.Role, result.Role),
                new Claim("AccessToken", result.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid username or password.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // ViewModel -> DTO Mapping
        var command = new RegisterCommand(
        
            model.Username,
            model.FullName,
            model.Email,
            model.Password,
            model.DepartmentId
        );

        var result = await authApiService.RegisterAsync(command);

        if (result != null && result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Registration successful! You can now login.";
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", result?.Message ?? "An error occurred during registration.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}