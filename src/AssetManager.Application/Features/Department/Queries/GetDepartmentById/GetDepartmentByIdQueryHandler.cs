using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AssetManager.Application.Features.Department.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(
    IDepartmentRepository departmentRepository,
    IMapper mapper) : IRequestHandler<GetDepartmentByIdQuery, DepartmentResponseDto?>
{
    public async Task<DepartmentResponseDto?> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdWithUsersAsync(request.Id);

        if (department == null) return null;

        // Entity -> DTO dönüşümü
        return mapper.Map<DepartmentResponseDto>(department);
    }
}