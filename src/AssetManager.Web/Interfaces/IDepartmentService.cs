using AssetManager.Application.DTOs.Department;

namespace AssetManager.Web.Interfaces
{
    public interface IDepartmentApiService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
        Task<bool> CreateAsync(CreateDepartmentRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
