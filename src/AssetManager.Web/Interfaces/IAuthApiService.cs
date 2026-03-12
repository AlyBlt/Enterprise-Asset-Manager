using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Features.Auth.Commands.Login;
using AssetManager.Application.Features.Auth.Commands.Register;

namespace AssetManager.Web.Interfaces;

public interface IAuthApiService
{
    Task<AuthResponseDto?> LoginAsync(LoginCommand loginRequest);
    Task<AuthResponseDto?> RegisterAsync(RegisterCommand registerRequest); 
}