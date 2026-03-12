using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;


namespace AssetManager.Application.Features.Department.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQueryHandler(
     IDepartmentRepository departmentRepository,
     IMapper mapper) : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponseDto>>
    {
        public async Task<IEnumerable<DepartmentResponseDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await departmentRepository.GetAllWithUsersAsync();
            return mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
        }
    }
}
