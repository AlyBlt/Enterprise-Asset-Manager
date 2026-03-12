using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AutoMapper;
using MediatR;


namespace AssetManager.Application.Features.Department.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IAuditLogService auditLogService) : IRequestHandler<CreateDepartmentCommand, bool>
    {
        public async Task<bool> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = mapper.Map<DepartmentEntity>(request);
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
    }
}
