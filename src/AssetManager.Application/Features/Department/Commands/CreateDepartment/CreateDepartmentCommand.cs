using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Department.Commands.CreateDepartment
{
    public record CreateDepartmentCommand(string Name, string Description) : IRequest<bool>;
}
