using AssetManager.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<AuthResponseDto>;
}
