using AssetManager.Application.DTOs.Dashboard;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace AssetManager.Application.Features.Dashboard.Queries.GetDashboardSummary
{
    public class GetDashboardSummaryQueryHandler(
     IAssetRepository assetRepository,
     IAuditLogRepository auditLogRepository,
     IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
    {
        public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var assets = await assetRepository.GetAllAsync();
            var activeAssets = assets.Where(a => !a.IsDeleted).ToList();

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
}
