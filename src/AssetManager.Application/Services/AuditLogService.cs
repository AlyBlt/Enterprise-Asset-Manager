using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AssetManager.Application.Services;

public class AuditLogService(
    IAuditLogRepository auditLogRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IAuditLogService
{
    public async Task LogAsync(string action, string entityName, string entityId, string details)
    {
        // IP ve Kullanıcı adını BURADA otomatik alıyoruz
        var userName = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";

        var log = new AuditLogEntity
        {
            UserName = userName,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details,
            IpAddress = ipAddress,
            TimestampUtc = DateTime.UtcNow
        };

        await auditLogRepository.AddLogAsync(log);
        await auditLogRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLogResponseDto>> GetAllLogsAsync()
    {
        var logs = await auditLogRepository.GetAllLogsAsync();
        // Entity listesini DTO listesine çeviriyoruz
        return mapper.Map<IEnumerable<AuditLogResponseDto>>(logs);
    }
}