using AssetManager.Application.DTOs.User;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Enums;
using AutoMapper;

namespace AssetManager.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    IAuditLogService auditLogService) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        // HasQueryFilter sayesinde !u.IsDeleted yazmamıza gerek kalmadı.
        // Artık repository'den direkt tüm aktif kullanıcıları çekebiliriz.
        var users = await userRepository.GetAllAsync();
        return mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return mapper.Map<UserDto>(user);
    }

    public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        if (Enum.TryParse<Roles>(newRole, true, out var role))
        {
            var oldRole = user.Role.ToString();
            user.Role = role;

            // user.UpdatedAt = DateTime.UtcNow; satırı artık gereksiz.
            // DbContext.SaveChangesAsync override metodu bunu otomatik yapacak!

            userRepository.Update(user);
            var result = await userRepository.SaveChangesAsync() > 0;

            if (result)
            {
                await auditLogService.LogAsync(
                    "Update-Role",
                    "AppUser",
                    userId.ToString(),
                    $"Role of {user.Username} updated from {oldRole} to {newRole}"
                );
            }
            return result;
        }
        return false;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null) return false;

        userRepository.Delete(user); // GenericRepo içindeki IsDeleted = true mantığı çalışır
        var result = await userRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Delete",
                "AppUser",
                id.ToString(),
                $"{user.Username} (ID: {id}) has been soft-deleted."
            );
        }
        return result;
    }
}