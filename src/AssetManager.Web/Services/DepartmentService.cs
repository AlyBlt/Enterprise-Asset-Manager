using AssetManager.Application.DTOs.Department;
using AssetManager.Web.Interfaces;
using System.Net.Http.Json;

namespace AssetManager.Web.Services
{
    public class DepartmentApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
     : BaseApiService(httpClient, httpContextAccessor), IDepartmentApiService
    {
        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/departments");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<IEnumerable<DepartmentResponseDto>>() ?? []
                : [];
        }

        public async Task<bool> CreateAsync(CreateDepartmentRequestDto request)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/departments", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/departments/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}