using AssetManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        // Sadece ekleme ve listeleme yetkisi veriyoruz (Log güvenliği)
        Task AddLogAsync(AuditLogEntity log);
        Task<IEnumerable<AuditLogEntity>> GetAllLogsAsync();
        Task<IEnumerable<AuditLogEntity>> GetLogsByEntityAsync(string entityName, string entityId);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<AuditLogEntity>> GetRecentLogsAsync(int count);
    }
}
