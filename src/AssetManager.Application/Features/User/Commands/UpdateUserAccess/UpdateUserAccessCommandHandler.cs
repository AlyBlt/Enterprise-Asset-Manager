using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Enums;
using MediatR;

namespace AssetManager.Application.Features.User.Commands.UpdateUserAccess;

public class UpdateUserAccessCommandHandler(
    IUserRepository userRepository,
    IAuditLogService auditLogService) : IRequestHandler<UpdateUserAccessCommand, bool>
{
    public async Task<bool> Handle(UpdateUserAccessCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcıyı Repository üzerinden getir
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null) return false;

        // 2. Rol Güncelleme (Enum kontrolü)
        if (!Enum.TryParse<Roles>(request.NewRole, true, out var role))
            return false;

        var oldRole = user.Role.ToString();
        var oldDeptId = user.DepartmentId;

        // Değerleri ata
        user.Role = role;
        user.DepartmentId = request.DepartmentId;

        // 3. Repository üzerinden güncelle ve kaydet
        userRepository.Update(user);
        var result = await userRepository.SaveChangesAsync() > 0;

        // 4. Loglama (Mevcut yapıdaki AuditLogService'i kullanıyoruz)
        if (result)
        {
            await auditLogService.LogAsync(
                "Update-Access",
                "AppUser",
                request.UserId.ToString(),
                $"User {user.Username} updated. Role: {oldRole}->{request.NewRole}, Dept: {oldDeptId}->{request.DepartmentId}");
        }

        return result;
    }
}