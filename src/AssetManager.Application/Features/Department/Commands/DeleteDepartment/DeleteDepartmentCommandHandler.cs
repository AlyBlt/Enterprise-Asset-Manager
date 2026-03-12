using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Department.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler(
     IDepartmentRepository departmentRepository,
     IAuditLogService auditLogService) : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await departmentRepository.GetByIdAsync(request.Id);
            if (department == null) return false;

            departmentRepository.Delete(department);
            var result = await departmentRepository.SaveChangesAsync() > 0;

            if (result)
            {
                await auditLogService.LogAsync(
                    "Delete", 
                    "Department", 
                    request.Id.ToString(), 
                    $"Department '{department.Name}' is soft-deleted.");
            }
            return result;
        }
    }
}
