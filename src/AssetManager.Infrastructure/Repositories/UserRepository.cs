using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Domain.Entities;
using AssetManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore; 
using System.Threading.Tasks;

namespace AssetManager.Infrastructure.Repositories
{
    internal class UserRepository(AssetManagerDbContext context)
    : GenericRepository<AppUserEntity>(context), IUserRepository
    {
        public async Task<AppUserEntity?> GetByUsernameWithDetailsAsync(string username)
        {
            return await _context.Users 
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }

        public async Task<IEnumerable<AppUserEntity>> GetAllWithDetailsAsync()
        {
            return await _context.Users
                .Include(u => u.Department) // Departman bilgilerini de çek
                .OrderByDescending(u => u.Id)
                .ToListAsync();
        }
    }
}