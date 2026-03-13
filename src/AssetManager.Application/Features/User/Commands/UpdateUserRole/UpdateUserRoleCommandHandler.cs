using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommandHandler(
      IUserRepository userRepository,
      IAuditLogService auditLogService) : IRequestHandler<UpdateUserRoleCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null) return false;

            if (Enum.TryParse<Roles>(request.NewRole, true, out var role))
            {
                var oldRole = user.Role.ToString();
                user.Role = role;

                userRepository.Update(user);
                var result = await userRepository.SaveChangesAsync() > 0;

                if (result)
                {
                    await auditLogService.LogAsync(
                        "Update-Role", 
                        "AppUser", 
                        request.UserId.ToString(),
                        $"Role of {user.Username} updated from {oldRole} to {request.NewRole}");
                }
                return result;
            }
            return false;
        }
    }
}
