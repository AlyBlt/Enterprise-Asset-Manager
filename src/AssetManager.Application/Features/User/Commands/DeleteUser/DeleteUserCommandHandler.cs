using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IAuditLogService auditLogService) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.Id);
            if (user == null) return false;

            userRepository.Delete(user);
            var result = await userRepository.SaveChangesAsync() > 0;

            if (result)
            {
                await auditLogService.LogAsync(
                    "Delete", 
                    "AppUser", 
                    request.Id.ToString(),
                    $"{user.Username} (ID: {request.Id}) has been soft-deleted.");
            }
            return result;
        }
    }
}
