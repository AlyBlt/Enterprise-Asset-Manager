using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Domain.Entities;
using AssetManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Repositories
{
    internal class AssetRepository(AssetManagerDbContext context)
    : GenericRepository<AssetEntity>(context), IAssetRepository
    {
        public async Task<IEnumerable<AssetEntity>> GetAllWithUserAsync()
        {
            return await _context.Assets
                .Include(a => a.AssignedUser) 
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }
        public async Task<AssetEntity?> GetWithUserByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.AssignedUser) // Kullanıcı tablosunu bağla
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
    }
}
