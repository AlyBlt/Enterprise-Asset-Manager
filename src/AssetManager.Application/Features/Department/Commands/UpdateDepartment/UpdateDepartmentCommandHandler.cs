using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using MediatR;

namespace AssetManager.Application.Features.Department.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IAuditLogService auditLogService) : IRequestHandler<UpdateDepartmentCommand, bool>
{
    public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Mevcut departmanı veritabanından çek
        var department = await departmentRepository.GetByIdAsync(request.Id);

        if (department == null) return false;

        // 2. Alanları güncelle
        department.Name = request.Name;
        department.Description = request.Description;

        // 3. Veritabanına yansıt
        departmentRepository.Update(department);
        var result = await departmentRepository.SaveChangesAsync() > 0;

        // 4. Başarılıysa Log at
        if (result)
        {
            await auditLogService.LogAsync(
                "Update",
                "Department",
                department.Name,
                $"Department updated: {department.Name} (ID: {department.Id})");
        }

        return result;
    }
}