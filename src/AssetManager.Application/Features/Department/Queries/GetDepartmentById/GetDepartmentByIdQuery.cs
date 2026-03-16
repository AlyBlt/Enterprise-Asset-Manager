using AssetManager.Application.DTOs.Department;
using MediatR;

namespace AssetManager.Application.Features.Department.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(int Id) : IRequest<DepartmentResponseDto?>;