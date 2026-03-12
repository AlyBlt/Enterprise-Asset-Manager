using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.UpdateUserRole
{
    public record UpdateUserRoleCommand(int UserId, string NewRole) : IRequest<bool>;
}
