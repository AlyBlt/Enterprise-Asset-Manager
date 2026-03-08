using AssetManager.Application.DTOs.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllDepartmentsAsync();
        Task<bool> CreateDepartmentAsync(CreateDepartmentRequestDto request);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
