using AssetManager.Application.Features.Asset.Commands.UpdateAsset;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using AutoMapper;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.Assets;

public class UpdateAssetCommandHandlerTests
{
    private readonly Mock<IAssetRepository> _mockAssetRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly UpdateAssetCommandHandler _handler;

    public UpdateAssetCommandHandlerTests()
    {
        _mockAssetRepo = new Mock<IAssetRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockAuditLogService = new Mock<IAuditLogService>();

        _handler = new UpdateAssetCommandHandler(
            _mockAssetRepo.Object,
            _mockMapper.Object,
            _mockAuditLogService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateAssetDetailsAndLogAction()
    {
        // --- 1. Hazırlık (Arrange) ---
        var assetId = 1;
        var command = new UpdateAssetCommand(assetId, "Updated Macbook", "M3 Chip 16GB", 2500.00m, "Electronics", "SN12345", 10);

        var existingAsset = new AssetEntity
        {
            Id = assetId,
            Name = "Old Macbook",
            Value = 2000.00m,
            Status = AssetStatus.InStock,
            SerialNumber = "OLD-SN"
        };

        // KRİTİK DÜZELTME: Handler GetByIdAsync çağırıyor, o yüzden onu kurmalıyız.
        _mockAssetRepo.Setup(x => x.GetByIdAsync(assetId))
            .ReturnsAsync(existingAsset);

        _mockAssetRepo.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // KRİTİK DÜZELTME: Mapper'ın boş dönmemesi için setup ekliyoruz.
        _mockMapper.Setup(m => m.Map(It.IsAny<UpdateAssetCommand>(), It.IsAny<AssetEntity>()))
            .Callback<object, object>((src, dest) =>
            {
                var s = (UpdateAssetCommand)src;
                var d = (AssetEntity)dest;
                d.Name = s.Name;
                d.Value = s.Price;
                d.Description = s.Description;
                d.Category = s.Category;
                d.SerialNumber = s.SerialNumber;
            });

        // --- 2. Çalıştırma (Act) ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Doğrulama (Assert) ---
        result.ShouldBeTrue();
        existingAsset.Name.ShouldBe("Updated Macbook");
        existingAsset.Status.ShouldBe(AssetStatus.Assigned); // ID 10 olduğu için Assigned olmalı

        _mockAssetRepo.Verify(x => x.Update(existingAsset), Times.Once);
        _mockAuditLogService.Verify(x => x.LogAsync("Update", "Asset", "SN12345", It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAssetNotFound_ShouldReturnFalse()
    {
        // Arrange
        _mockAssetRepo.Setup(x => x.GetWithUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((AssetEntity)null!);

        var command = new UpdateAssetCommand(999, "None", "None", 0, "None", "None", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
        _mockAssetRepo.Verify(x => x.Update(It.IsAny<AssetEntity>()), Times.Never);
    }
}