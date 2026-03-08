using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Services
{
    public interface IAuditLogService
    {
        // Manuel log eklemek için (Örn: "Kullanıcı hatalı şifre girdi")
        Task LogAsync(string action, string entityName, string entityId, string details);

        // Admin panelinde listelemek için
        Task<IEnumerable<AuditLogResponseDto>> GetAllLogsAsync();
    }
}
