using AssetManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUserEntity>
    {
        Task<AppUserEntity?> GetByUsernameWithDetailsAsync(string username);
        Task<IEnumerable<AppUserEntity>> GetAllWithDetailsAsync();
    }
}
