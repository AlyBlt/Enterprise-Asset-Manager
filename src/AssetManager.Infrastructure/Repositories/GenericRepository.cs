using AssetManager.Core.Entities;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AssetManager.Infrastructure.Repositories;

internal class GenericRepository<T>(AssetManagerDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly AssetManagerDbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        // Entity Framework takip mekanizması (Tracking) sayesinde 
        // Update aslında nesne üzerindeki değişiklikleri işaretler.
    }

    public void Delete(T entity)
    {
        // Gerçek silme yerine Soft Delete yapıyoruz
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);

    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}