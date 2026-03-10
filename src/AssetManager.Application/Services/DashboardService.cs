using AssetManager.Application.DTOs.Dashboard;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace AssetManager.Application.Services;

public class DashboardService(
    IAssetRepository assetRepository,
    IAuditLogRepository auditLogRepository,
    IHttpContextAccessor httpContextAccessor) : IDashboardService
{
    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var assets = await assetRepository.GetAllAsync();
        var activeAssets = assets.Where(a => !a.IsDeleted).ToList();
        // Kullanıcı Admin mi kontrol et
        var isAdmin = httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;

        var summary = new DashboardSummaryDto
        {
            TotalAssets = activeAssets.Count,
            AssignedAssets = activeAssets.Count(a => a.Status == AssetStatus.Assigned),
            InStockAssets = activeAssets.Count(a => a.Status == AssetStatus.InStock),
            LostAssets = activeAssets.Count(a => a.Status == AssetStatus.Lost),
            ActiveAsset = activeAssets.Count(a => a.Status == AssetStatus.Active),
            InRepairAsset = activeAssets.Count(a => a.Status == AssetStatus.InRepair),
            RetiredAsset = activeAssets.Count(a => a.Status == AssetStatus.Retired),
            RecentActivities = new List<RecentActivityDto>()
        };

        // SADECE Admin ise logları ekle
        if (isAdmin)
        {
            var logs = await auditLogRepository.GetRecentLogsAsync(5);
            summary.RecentActivities = logs.Select(l => new RecentActivityDto
            {
                Description = l.Details,
                Date = l.TimestampUtc.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
                ActionType = l.Action
            }).ToList();
        }

        return summary;

    }
}