using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Core.Entities;
using AssetManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Repositories
{
    internal class DepartmentRepository(AssetManagerDbContext context)
    : GenericRepository<DepartmentEntity>(context), IDepartmentRepository
    {
        public async Task<IEnumerable<DepartmentEntity>> GetAllWithUsersAsync()
        {
            // _context'e GenericRepository içinden (protected olduğu için) erişebiliyoruz.
            return await _context.Departments
                .Include(d => d.Users) // JOIN işlemi burada gerçekleşiyor
                .Where(d => !d.IsDeleted) // BaseEntity'den gelen IsDeleted kontrolü
                .ToListAsync();
        }
    }
}
