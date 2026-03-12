using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.DeleteUser
{
    public record DeleteUserCommand(int Id) : IRequest<bool>;
}
