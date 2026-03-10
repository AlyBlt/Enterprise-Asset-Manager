using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Core.Entities;
using AssetManager.Core.Enums;
using AutoMapper;
using FluentValidation; // Validator için eklendi

namespace AssetManager.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IPasswordHasher passwordHasher,
    IMapper mapper,
    IAuditLogService auditLogService,
    IValidator<RegisterRequestDto> registerValidator, // Enjekte edildi
    IValidator<LoginRequestDto> loginValidator) : IAuthService // Enjekte edildi
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // 1. VALIDASYON KONTROLÜ
        var validationResult = await registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            // Exception fırlatmak yerine mesajları birleştirip dönüyoruz
            var errorMessages = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new AuthResponseDto { IsSuccess = false, Message = errorMessages };
        }

        var users = await userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);

        if (users.Any())
        {
            return new AuthResponseDto { IsSuccess = false, Message = "This username or email is already used." };
        }

        var newUser = mapper.Map<AppUserEntity>(request);
        newUser.PasswordHash = passwordHasher.HashPassword(request.Password);
        newUser.Role = Roles.Guest;

        await userRepository.AddAsync(newUser);
        var result = await userRepository.SaveChangesAsync() > 0;

        if (result)
        {
            await auditLogService.LogAsync(
                "Register",
                "AppUser",
                newUser.Username,
                $"New user registered: {newUser.Username}"
            );
        }

        return new AuthResponseDto { IsSuccess = true, Message = "Successfully registered." };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        // 1. VALIDASYON KONTROLÜ
        var validationResult = await loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new AuthResponseDto { IsSuccess = false, Message = errorMessages };
        }

        var user = await userRepository.GetByUsernameWithDetailsAsync(request.Username);

        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            await auditLogService.LogAsync(
                "Login-Failed",
                "AppUser",
                request.Username,
                "Invalid login attempt!"
            );

            return new AuthResponseDto { IsSuccess = false, Message = "Wrong username or password!" };
        }

        var token = tokenService.GenerateJwtToken(user);

        await auditLogService.LogAsync(
            "Login-Success",
            "AppUser",
            user.Id.ToString(),
            $"User signed in: {user.Username}"
        );

        return new AuthResponseDto
        {
            IsSuccess = true,
            Token = token,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }
}