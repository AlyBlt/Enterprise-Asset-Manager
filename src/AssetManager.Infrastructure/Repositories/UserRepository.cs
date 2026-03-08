using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Core.Entities;
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
    }
}