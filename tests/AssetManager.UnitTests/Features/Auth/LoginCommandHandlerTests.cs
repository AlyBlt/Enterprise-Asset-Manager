using AssetManager.Application.DTOs.Auth;
using AssetManager.Application.Features.Auth.Commands.Login;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockAuditLogService = new Mock<IAuditLogService>();

        _handler = new LoginCommandHandler(
            _mockUserRepo.Object,
            _mockTokenService.Object,
            _mockPasswordHasher.Object,
            _mockAuditLogService.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnSuccessResponse()
    {
        // --- 1. Arrange ---
        var command = new LoginCommand("admin", "123456");
        var user = new AppUserEntity
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "hashed_password",
            Role = Roles.Admin
        };

        _mockUserRepo.Setup(x => x.GetByUsernameWithDetailsAsync(command.Username))
                     .ReturnsAsync(user);

        _mockPasswordHasher.Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
                           .Returns(true);

        _mockTokenService.Setup(x => x.GenerateJwtToken(user))
                         .Returns("fake_jwt_token");

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.IsSuccess.ShouldBeTrue();
        result.Token.ShouldBe("fake_jwt_token");
        result.Username.ShouldBe("admin");

        _mockAuditLogService.Verify(x => x.LogAsync("Login-Success", "AppUser", "1", It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ShouldReturnFailResponse()
    {
        // --- 1. Arrange ---
        var command = new LoginCommand("admin", "wrong_password");
        var user = new AppUserEntity { Username = "admin", PasswordHash = "hashed_password" };

        _mockUserRepo.Setup(x => x.GetByUsernameWithDetailsAsync(command.Username))
                     .ReturnsAsync(user);

        // Şifre yanlış dönsün
        _mockPasswordHasher.Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
                           .Returns(false);

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.IsSuccess.ShouldBeFalse();
        result.Message.ShouldBe("Wrong username or password!");

        _mockAuditLogService.Verify(x => x.LogAsync("Login-Failed", "AppUser", "admin", It.IsAny<string>()), Times.Once);
        _mockTokenService.Verify(x => x.GenerateJwtToken(It.IsAny<AppUserEntity>()), Times.Never);
    }
}