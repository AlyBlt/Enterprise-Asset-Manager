using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Features.Department.Commands.CreateDepartment;

namespace AssetManager.Web.Interfaces
{
    public interface IDepartmentApiService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
        Task<bool> CreateAsync(CreateDepartmentCommand request);
        Task<bool> DeleteAsync(int id);
    }
}
