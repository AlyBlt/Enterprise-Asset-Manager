using AssetManager.Application.Features.AuditLog.Queries.GetAllAuditLogs;
using AssetManager.Application.Features.User.Commands.DeleteUser;
using AssetManager.Application.Features.User.Commands.UpdateUserAccess;
using AssetManager.Application.Features.User.Commands.UpdateUserRole;
using AssetManager.Application.Features.User.Queries.GetAllUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : BaseController
{
    // 1. Tüm kullanıcıları getir (Query)
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await Mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    // 2. Kullanıcı rolünü güncelle (Command)
    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] string newRole)
    {
        var result = await Mediator.Send(new UpdateUserRoleCommand(id, newRole));
        if (!result) return BadRequest("User not found or invalid role.");

        return Ok(new { Message = "User role updated successfully." });
    }

    // 3. Kullanıcıyı sil (Command)
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> RemoveUser(int id)
    {
        var result = await Mediator.Send(new DeleteUserCommand(id));
        if (!result) return NotFound();

        return Ok(new { Message = "User deleted successfully." });
    }

    // 4. Tüm logları getir (Query)
    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs()
    {
        var logs = await Mediator.Send(new GetAllAuditLogsQuery());
        return Ok(logs);
    }

    [HttpPut("users/{id}/access")]
    public async Task<IActionResult> UpdateUserAccess(int id, [FromBody] UpdateUserAccessCommand request)
    {
        if (id != request.UserId) return BadRequest("ID mismatch");

        var result = await Mediator.Send(request);

        return result ? Ok(new { Message = "Access updated." }) : BadRequest("Update failed.");
    }
}