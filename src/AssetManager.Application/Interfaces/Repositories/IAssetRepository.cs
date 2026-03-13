using AssetManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Repositories
{
    public interface IAssetRepository : IGenericRepository<AssetEntity>
    {
        // Zimmetlenen kullanıcı bilgisiyle beraber getirmek için özel metod
        Task<IEnumerable<AssetEntity>> GetAllWithUserAsync();
        Task<AssetEntity?> GetWithUserByIdAsync(int id);
    }
}
