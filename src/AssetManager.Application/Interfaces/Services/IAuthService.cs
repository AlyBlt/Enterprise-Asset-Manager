using AssetManager.Application.DTOs;
using AssetManager.Application.DTOs.Auth;

namespace AssetManager.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
}