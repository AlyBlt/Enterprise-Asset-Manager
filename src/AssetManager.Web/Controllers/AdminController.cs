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
}