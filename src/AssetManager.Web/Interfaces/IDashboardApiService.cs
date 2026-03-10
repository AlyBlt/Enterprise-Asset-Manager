using AssetManager.Web.Models.Dashboard;

namespace AssetManager.Web.Interfaces
{
    public interface IDashboardApiService
    {
        // API'den özet istatistikleri getirecek metod
        Task<DashboardViewModel?> GetDashboardSummaryAsync();
    }
}
