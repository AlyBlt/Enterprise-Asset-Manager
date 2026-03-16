using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IAssetRepository assetRepository,
    IAuditLogService auditLogService) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.Id);
            if (user == null) return false;

            // Bu kullanıcıya atanmış cihazları bul (Repository'deki tüm cihazları getirip filtreliyoruz)
            var allAssets = await assetRepository.GetAllWithUserAsync();
            var assignedAssets = allAssets.Where(a => a.AssignedUserId == request.Id).ToList();

            foreach (var asset in assignedAssets)
            {
                // Kullanıcı silindiği için cihazı boşa çıkar ve statüsünü güncelle
                asset.AssignedUserId = null;
                asset.Status = AssetStatus.InStock; // Artık veritabanında da 'InStock' olacak
                assetRepository.Update(asset);
            }

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
