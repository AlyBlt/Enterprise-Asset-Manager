using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Department.Commands.DeleteDepartment
{
    public record DeleteDepartmentCommand(int Id) : IRequest<bool>;
}
