using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.DTOs.User;
using AssetManager.Web.Interfaces;
using System.Net.Http.Json;

namespace AssetManager.Web.Services;

public class AdminApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    : BaseApiService(httpClient, httpContextAccessor), IAdminApiService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync("api/admin/users");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>() ?? []
            : [];
    }

    public async Task<(bool IsSuccess, string? Message)> ChangeUserRoleAsync(int userId, string newRole)
    {
        AddAuthorizationHeader();
        // API tarafındaki endpoint yapına göre role bilgisini query veya body olarak gönderiyoruz
        var response = await _httpClient.PutAsJsonAsync($"api/admin/users/{userId}/role", newRole);
        if (response.IsSuccessStatusCode)
        {
            return (true, "Role updated successfully.");
        }
        // API'den gelen hata detayını oku (Middleware'in fırlattığı JSON)
        var errorJson = await response.Content.ReadAsStringAsync();
        return (false, errorJson);
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"api/admin/users/{userId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<AuditLogResponseDto>> GetAllLogsAsync()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync("api/admin/logs");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<IEnumerable<AuditLogResponseDto>>() ?? []
            : [];
    }
}