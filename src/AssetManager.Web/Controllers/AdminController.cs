using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.AuditLog;
using AssetManager.Web.Models.User;
using AssetManager.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

[Authorize(Roles = "Admin")] // Tüm sayfa sadece Admin'e özel
public class AdminController(IAdminApiService adminApiService, IDepartmentApiService departmentApiService) : Controller
{
    // 1. Kullanıcı Listesi
    public async Task<IActionResult> Users(int? departmentId)
    {
        var users = await adminApiService.GetAllUsersAsync();
        var departments = await departmentApiService.GetAllAsync();
        ViewBag.Departments = await departmentApiService.GetAllAsync();

        // Filtreleme mantığı
        if (departmentId.HasValue)
        {
            users = users.Where(u => u.DepartmentId == departmentId.Value);
            ViewBag.SelectedDepartmentId = departmentId;
        }

        var model = new UserListViewModel
        {
            Users = users,
            Departments = departments 
        };

        return View(model);
    }

    // 2. Rol Değiştirme
    [HttpPost]
    public async Task<IActionResult> ChangeRole(int userId, string newRole)
    {
        // Artık Tuple dönüyoruz: (bool, string)
        var (isSuccess, message) = await adminApiService.ChangeUserRoleAsync(userId, newRole);

        if (isSuccess)
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            // API'den gelen o detaylı "Validation Failed" mesajı burada görünecek!
            TempData["ErrorMessage"] = "API Error: " + message;
        }

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

    [HttpPost]
    public async Task<IActionResult> UpdateUserAccess(int userId, string newRole, int? departmentId)
    {
        var result = await adminApiService.UpdateUserPermissionsAsync(userId, newRole, departmentId);

        if (result)
        {
            TempData["SuccessMessage"] = "Personnel settings have been updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to update user settings. Please check API connection.";
        }

        return RedirectToAction(nameof(Users));
    }
}