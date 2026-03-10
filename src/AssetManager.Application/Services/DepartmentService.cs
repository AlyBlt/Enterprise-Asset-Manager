using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AutoMapper;
using FluentValidation; // Validator kullanımı için eklendi

namespace AssetManager.Application.Services;

public class DepartmentService(
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IAuditLogService auditLogService,
    IValidator<CreateDepartmentRequestDto> departmentValidator) : IDepartmentService // Validator eklendi
{
    public async Task<IEnumerable<DepartmentResponseDto>> GetAllDepartmentsAsync()
    {
        var departments = await departmentRepository.GetAllWithUsersAsync();

        return mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
    }

    public async Task<bool> CreateDepartmentAsync(CreateDepartmentRequestDto request)
    {
        // --- VALIDASYON KONTROLÜ ---
        var validationResult = await departmentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            // GlobalExceptionMiddleware bu hatayı yakalayıp 400 Bad Request dönecek.
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        // ---------------------------

        var department = mapper.Map<DepartmentEntity>(request);

        // CreatedAt artık DbContext (BaseEntity döngüsü) tarafından otomatik set ediliyor.

        await departmentRepository.AddAsync(department);
        var result = await departmentRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Create",
                "Department",
                department.Name,
                $"New department is created: {department.Name}");
        }
        return result;
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        var department = await departmentRepository.GetByIdAsync(id);
        if (department == null) return false;

        departmentRepository.Delete(department); // GenericRepository içindeki Soft Delete mantığı çalışır.
        var result = await departmentRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Delete",
                "Department",
                id.ToString(),
                $"Department '{department.Name}' is soft-deleted.");
        }
        return result;
    }
}