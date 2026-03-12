using AssetManager.Application.DTOs.Department;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Department.Queries.GetAllDepartments
{
    public record GetAllDepartmentsQuery() : IRequest<IEnumerable<DepartmentResponseDto>>;
}
