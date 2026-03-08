using AssetManager.Application.DTOs.User;
using AssetManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserRoleAsync(int userId, string newRole);
        Task<bool> DeleteUserAsync(int id);
    }
}
