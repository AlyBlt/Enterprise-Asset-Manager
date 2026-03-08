using AssetManager.Application.Interfaces.Services;
using AssetManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[Authorize(Roles = "Admin")] // Tüm controller sadece Admin'e açık
[ApiController]
[Route("api/[controller]")]
public class AdminController(IUserService userService, IAuditLogService auditLogService) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] string newRole)
    {
        var result = await userService.UpdateUserRoleAsync(id, newRole);
        if (!result) return BadRequest("User not found or invalid role.");

        return Ok(new { Message = "User role updated successfully." });
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> RemoveUser(int id)
    {
        var result = await userService.DeleteUserAsync(id);
        if (!result) return NotFound();

        return Ok(new { Message = "User deleted successfully." });
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs()
    {
        var logs = await auditLogService.GetAllLogsAsync();
        return Ok(logs);
    }
}