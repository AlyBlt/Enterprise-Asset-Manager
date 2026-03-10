namespace AssetManager.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int InStockAssets { get; set; }
        public int LostAssets { get; set; }
        public int ActiveAsset { get; set; }
        public int InRepairAsset { get; set; }
        public int RetiredAsset { get; set; }
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
    }
}
