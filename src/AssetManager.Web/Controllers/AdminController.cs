using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.AuditLog;
using AssetManager.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

[Authorize(Roles = "Admin")] // Tüm sayfa sadece Admin'e özel
public class AdminController(IAdminApiService adminApiService) : Controller
{
    // 1. Kullanıcı Listesi
    public async Task<IActionResult> Users(int? departmentId)
    {
        var users = await adminApiService.GetAllUsersAsync();

        // Filtreleme mantığı
        if (departmentId.HasValue)
        {
            users = users.Where(u => u.DepartmentId == departmentId.Value);
            ViewBag.SelectedDepartmentId = departmentId;
        }

        var model = new UserListViewModel { Users = users };
        return View(model);
    }

    // 2. Rol Değiştirme (İşlem)
    [HttpPost]
    public async Task<IActionResult> ChangeRole(int userId, string newRole)
    {
        var result = await adminApiService.ChangeUserRoleAsync(userId, newRole);
        if (result)
            TempData["SuccessMessage"] = "User role updated successfully.";
        else
            TempData["ErrorMessage"] = "Failed to update user role.";

        return RedirectToAction(nameof(Users));
    }

    // 3. Kullanıcı Silme
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await adminApiService.DeleteUserAsync(userId);
        return RedirectToAction(nameof(Users));
    }

    // 4. Sistem Logları (Audit Logs)
    public async Task<IActionResult> Logs()
    {
        var logs = await adminApiService.GetAllLogsAsync();
        var model = new AuditLogViewModel { Logs = logs };
        return View(model);
    }
}