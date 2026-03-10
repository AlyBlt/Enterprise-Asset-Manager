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
            //yetki gerekliyse bunu ekliyoruz
            AddAuthorizationHeader();

            var response = await _httpClient.GetAsync("api/dashboard/summary");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DashboardViewModel>();
            }
            return null;
        }
        catch (Exception)
        {
            // Loglama mekanizması eklenebilir
            return null;
        }
    }
}