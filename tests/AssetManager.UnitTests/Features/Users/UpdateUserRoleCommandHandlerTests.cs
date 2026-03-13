using AssetManager.Application.Features.User.Commands.UpdateUserRole;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.Users;

public class UpdateUserRoleCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly UpdateUserRoleCommandHandler _handler;

    public UpdateUserRoleCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockAuditLogService = new Mock<IAuditLogService>();

        _handler = new UpdateUserRoleCommandHandler(
            _mockUserRepo.Object,
            _mockAuditLogService.Object);
    }

    [Fact]
    public async Task Handle_UserExistsAndRoleIsValid_ShouldUpdateRoleAndReturnTrue()
    {
        // --- 1. Arrange ---
        var command = new UpdateUserRoleCommand(1, "Admin");
        var user = new AppUserEntity
        {
            Id = 1,
            Username = "johndoe",
            Role = Roles.Guest 
        };

        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId))
                     .ReturnsAsync(user);

        _mockUserRepo.Setup(x => x.SaveChangesAsync())
                     .ReturnsAsync(1); // Başarılı kayıt

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldBeTrue();
        user.Role.ShouldBe(Roles.Admin); // Rol değişmiş mi?

        _mockUserRepo.Verify(x => x.Update(user), Times.Once);
        _mockAuditLogService.Verify(x => x.LogAsync(
           "Update-Role",
           "AppUser",
           "1",
           // Eskiden Guest idi, şimdi Admin oldu; ikisinin de mesajda geçtiğini kontrol ediyoruz
           It.Is<string>(s => s.Contains(Roles.Guest.ToString()) && s.Contains("Admin"))),
           Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldReturnFalse()
    {
        // --- 1. Arrange ---
        var command = new UpdateUserRoleCommand(999, "Admin");

        // Kullanıcı bulunamasın (null dönsün)
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId))
                     .ReturnsAsync((AppUserEntity)null!);

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldBeFalse();

        // Kullanıcı yoksa güncelleme ve loglama asla yapılmamalı
        _mockUserRepo.Verify(x => x.Update(It.IsAny<AppUserEntity>()), Times.Never);
        _mockAuditLogService.Verify(x => x.LogAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidRoleName_ShouldReturnFalse()
    {
        // --- 1. Arrange ---
        // Enum içinde karşılığı olmayan saçma bir rol gönderelim
        var command = new UpdateUserRoleCommand(1, "SuperHeroRole");
        var user = new AppUserEntity { Id = 1, Username = "johndoe", Role = Roles.Guest };

        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId))
                     .ReturnsAsync(user);

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldBeFalse();

        // Rol geçersiz olduğu için DB'ye yansımamalı
        _mockUserRepo.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}