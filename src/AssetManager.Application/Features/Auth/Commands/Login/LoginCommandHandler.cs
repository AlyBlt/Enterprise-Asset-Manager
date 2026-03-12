using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler(
     IUserRepository userRepository,
     ITokenService tokenService,
     IPasswordHasher passwordHasher,
     IAuditLogService auditLogService) : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcıyı bul
            var user = await userRepository.GetByUsernameWithDetailsAsync(request.Username);

            // 2. Kullanıcı yoksa veya şifre yanlışsa
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

            // 3. Token Üret
            var token = tokenService.GenerateJwtToken(user);

            // 4. Başarılı Logu
            await auditLogService.LogAsync(
                "Login-Success",
                "AppUser",
                user.Id.ToString(),
                $"User signed in: {user.Username}"
            );

            // 5. Yanıtı dön
            return new AuthResponseDto
            {
                IsSuccess = true,
                Token = token,
                Username = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}
