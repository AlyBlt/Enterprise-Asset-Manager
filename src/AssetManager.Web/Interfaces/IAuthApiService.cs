using AssetManager.Application.DTOs.Auth;

namespace AssetManager.Web.Interfaces;

public interface IAuthApiService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequest); 
}