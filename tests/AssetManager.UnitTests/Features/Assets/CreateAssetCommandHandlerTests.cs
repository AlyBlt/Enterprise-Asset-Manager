using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using AutoMapper;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.Assets;

public class CreateAssetCommandHandlerTests
{
    private readonly Mock<IAssetRepository> _mockAssetRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly CreateAssetCommandHandler _handler;

    public CreateAssetCommandHandlerTests()
    {
        // 1. Arrange: Bağımlılıkları Mock'la
        _mockAssetRepo = new Mock<IAssetRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockAuditLogService = new Mock<IAuditLogService>();

        // Handler'ı bu mock nesnelerle ayağa kaldır
        _handler = new CreateAssetCommandHandler(
            _mockAssetRepo.Object,
            _mockMapper.Object,
            _mockAuditLogService.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateAssetAndLogAudit()
    {
        // --- 1. Hazırlık (Arrange) ---
        var command = new CreateAssetCommand(
            "Test Monitor",
            "High resolution monitor",
            1500.00m,
            "IT",
            "MON-001");

        var assetEntity = new AssetEntity
        {
            Id = 1,
            Name = command.Name,
            SerialNumber = command.SerialNumber,
            Description = command.Description,
            Category = command.Category,
            Value = command.Price
        };

        // Mapper taklidi: Command gelince bu Entity'yi dön
        _mockMapper.Setup(m => m.Map<AssetEntity>(It.IsAny<CreateAssetCommand>()))
               .Returns(assetEntity);

        // Repo taklidi: SaveChangesAsync çağrılınca 1 dön (Başarılı)
        _mockAssetRepo.Setup(x => x.SaveChangesAsync())
                   .ReturnsAsync(1);

        // --- 2. Çalıştırma (Act) ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Doğrulama (Assert) ---
        result.ShouldBeTrue(); // Handler true dönmeli

        // AssetRepository.AddAsync metodu en az bir kere çağrıldı mı?
        _mockAssetRepo.Verify(x => x.AddAsync(It.IsAny<AssetEntity>()), Times.Once);

        // AuditLogService.LogAsync metodu çağrıldı mı?
        _mockAuditLogService.Verify(x => x.LogAsync(
            "Create",
            "Asset",
            assetEntity.SerialNumber,
            It.IsAny<string>()), Times.Once);

        // Status InStock olarak setlendi mi?
        assetEntity.Status.ShouldBe(AssetStatus.InStock);
    }
}