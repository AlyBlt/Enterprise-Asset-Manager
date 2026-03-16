using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Features.Department.Commands.CreateDepartment;
using AssetManager.Application.Features.Department.Commands.UpdateDepartment;
using AssetManager.Web.Interfaces;

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

        public async Task<bool> CreateAsync(CreateDepartmentCommand request)
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

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"api/departments/{id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<DepartmentResponseDto>();

            return null;
        }

        public async Task<bool> UpdateAsync(UpdateDepartmentCommand request)
        {
            AddAuthorizationHeader();
            // API Controller'da [HttpPut("{id}")] beklediğimiz için URL'ye id ekliyoruz
            var response = await _httpClient.PutAsJsonAsync($"api/departments/{request.Id}", request);
            return response.IsSuccessStatusCode;
        }


    }
}