using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int InStockAssets { get; set; }
        public int LostAssets { get; set; }
        public int ActiveAsset { get; set; }
        public int InRepairAsset { get; set; }
        public int RetiredAsset { get; set; }

        public List<RecentActivityDto> RecentActivities { get; set; } = new();

    }
}
