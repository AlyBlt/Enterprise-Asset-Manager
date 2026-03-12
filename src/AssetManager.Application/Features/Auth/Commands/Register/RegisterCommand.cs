using AssetManager.Application.DTOs.Auth;
using MediatR;


namespace AssetManager.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(string Username, string Email, string Password, string FullName, int? DepartmentId)
    : IRequest<AuthResponseDto>;
}
