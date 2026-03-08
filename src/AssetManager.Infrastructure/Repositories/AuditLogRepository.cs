using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Core.Entities;
using AssetManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Infrastructure.Repositories;

internal class AuditLogRepository(AssetManagerDbContext context) : IAuditLogRepository
{
    private readonly DbSet<AuditLogEntity> _dbSet = context.Set<AuditLogEntity>();

    public async Task AddLogAsync(AuditLogEntity log)
    {
        await _dbSet.AddAsync(log);
    }

    public async Task<IEnumerable<AuditLogEntity>> GetAllLogsAsync()
    {
        // Logları her zaman en yeniden en eskiye doğru sıralı getirelim
        return await _dbSet.OrderByDescending(x => x.TimestampUtc).ToListAsync();
    }

    public async Task<IEnumerable<AuditLogEntity>> GetLogsByEntityAsync(string entityName, string entityId)
    {
        return await _dbSet
            .Where(x => x.EntityName == entityName && x.EntityId == entityId)
            .OrderByDescending(x => x.TimestampUtc)
            .ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}