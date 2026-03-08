using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AssetManager.Core.Enums;

namespace AssetManager.Application.Services;

public class AuthService(
    IGenericRepository<AppUserEntity> userRepository,
    ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // 1. Kullanıcı var mı kontrol et
        var existingUser = await userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);
        if (existingUser.Any())
            return new AuthResponseDto { IsSuccess = false, Message = "Kullanıcı adı veya e-posta zaten kullanımda!" };

        // 2. Yeni kullanıcıyı oluştur
        var newUser = new AppUserEntity
        {
            Username = request.Username,
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = request.Password, // İleride BCrypt ile hashleyeceğiz
            Role = Roles.Guest, // Yeni kayıt olanlar varsayılan olarak Guest
            DepartmentId = request.DepartmentId
        };

        await userRepository.AddAsync(newUser);
        await userRepository.SaveChangesAsync();

        return new AuthResponseDto { IsSuccess = true, Message = "Kayıt başarıyla tamamlandı." };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var users = await userRepository.FindAsync(u => u.Username == request.Username);
        var user = users.FirstOrDefault();

        if (user == null || user.PasswordHash != request.Password)
            return new AuthResponseDto { IsSuccess = false, Message = "Hatalı giriş bilgileri!" };

        // Ayrıştırdığımız TokenService'i çağırıyoruz
        var token = tokenService.GenerateJwtToken(user);

        return new AuthResponseDto
        {
            IsSuccess = true,
            Token = token,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }
}