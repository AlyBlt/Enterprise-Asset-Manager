using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.DTOs.User;

namespace AssetManager.Web.Interfaces
{
    public interface IAdminApiService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<(bool IsSuccess, string? Message)> ChangeUserRoleAsync(int userId, string newRole);
        Task<bool> DeleteUserAsync(int userId);
        Task<IEnumerable<AuditLogResponseDto>> GetAllLogsAsync();
    }
}
