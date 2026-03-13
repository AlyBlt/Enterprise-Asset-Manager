using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using AutoMapper;
using MediatR;


namespace AssetManager.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IMapper mapper,
    IAuditLogService auditLogService) : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı var mı kontrolü
            var existingUsers = await userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (existingUsers.Any())
            {
                return new AuthResponseDto { IsSuccess = false, Message = "This username or email is already used." };
            }

            // Mapping ve Şifreleme
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
                    $"New user registered: {newUser.Username}");
            }

            return new AuthResponseDto { IsSuccess = true, Message = "Successfully registered." };
        }
    }
}
