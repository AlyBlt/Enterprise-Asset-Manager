using AssetManager.Application.Features.User.Commands.UpdateUserAccess;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.Users;

public class UpdateUserAccessCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly UpdateUserAccessCommandHandler _handler;

    public UpdateUserAccessCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockAuditLogService = new Mock<IAuditLogService>();

        _handler = new UpdateUserAccessCommandHandler(
            _mockUserRepo.Object,
            _mockAuditLogService.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateUserAccessAndLogAudit()
    {
        // --- 1. Hazırlık (Arrange) ---
        var userId = 1;
        var command = new UpdateUserAccessCommand(userId, "Admin", 5);

        var existingUser = new AppUserEntity
        {
            Id = userId,
            Username = "jdoe",
            Role = Roles.Guest, // Eski rolü
            DepartmentId = 1     // Eski departmanı
        };

        // Repository taklidi: Kullanıcıyı bulup getir
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId))
                    .ReturnsAsync(existingUser);

        // Repo taklidi: SaveChanges başarılı
        _mockUserRepo.Setup(x => x.SaveChangesAsync())
                    .ReturnsAsync(1);

        // --- 2. Çalıştırma (Act) ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Doğrulama (Assert) ---
        result.ShouldBeTrue();

        // Rol ve Departman doğru güncellendi mi?
        existingUser.Role.ShouldBe(Roles.Admin);
        existingUser.DepartmentId.ShouldBe(5);

        // Update metodu çağrıldı mı?
        _mockUserRepo.Verify(x => x.Update(existingUser), Times.Once);

        // Audit log atıldı mı?
        _mockAuditLogService.Verify(x => x.LogAsync(
            "Update-Access",
            "AppUser",
            userId.ToString(),
            It.Is<string>(s => s.Contains("Admin") && s.Contains("5"))), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldReturnFalse()
    {
        // Arrange
        _mockUserRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((AppUserEntity)null!);

        var command = new UpdateUserAccessCommand(99, "Admin", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
        _mockUserRepo.Verify(x => x.Update(It.IsAny<AppUserEntity>()), Times.Never);
    }
}