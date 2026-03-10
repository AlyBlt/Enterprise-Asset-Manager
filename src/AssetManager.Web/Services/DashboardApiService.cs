using AssetManager.Application.DTOs.Dashboard;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Dashboard;
using System.Net.Http.Json;

namespace AssetManager.Web.Services;

public class DashboardApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    : BaseApiService(httpClient, httpContextAccessor), IDashboardApiService
{
    public async Task<DashboardViewModel?> GetDashboardSummaryAsync()
    {
        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/dashboard/summary");

            if (response.IsSuccessStatusCode)
            {
                // 1. API'den gelen orijinal DTO'yu oku
                var dto = await response.Content.ReadFromJsonAsync<DashboardSummaryDto>();

                if (dto == null) return null;

                // 2. DTO'yu Web projesinin anladığı ViewModel'e çevir
                return new DashboardViewModel
                {
                    TotalAssets = dto.TotalAssets,
                    AssignedAssets = dto.AssignedAssets,
                    InStockAssets = dto.InStockAssets,
                    LostAssets = dto.LostAssets,
                    ActiveAsset = dto.ActiveAsset,
                    InRepairAsset = dto.InRepairAsset,
                    RetiredAsset = dto.RetiredAsset,
                    RecentActivities = dto.RecentActivities.Select(a => new RecentActivityViewModel
                    {
                        Description = a.Description,
                        Date = a.Date,
                        ActionType = a.ActionType
                    }).ToList()
                };
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}