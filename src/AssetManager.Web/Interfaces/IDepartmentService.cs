using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Features.Department.Commands.CreateDepartment;
using AssetManager.Application.Features.Department.Commands.UpdateDepartment;

namespace AssetManager.Web.Interfaces
{
    public interface IDepartmentApiService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
        Task<bool> CreateAsync(CreateDepartmentCommand request);
        Task<bool> DeleteAsync(int id);
        Task<DepartmentResponseDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(UpdateDepartmentCommand request);
    }
}
