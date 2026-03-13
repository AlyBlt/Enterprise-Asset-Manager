using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.Features.AuditLog.Queries.GetAllAuditLogs;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Domain.Entities;
using AutoMapper;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.AuditLogs;

public class GetAllAuditLogsQueryHandlerTests
{
    private readonly Mock<IAuditLogRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllAuditLogsQueryHandler _handler;

    public GetAllAuditLogsQueryHandlerTests()
    {
        _mockRepo = new Mock<IAuditLogRepository>();
        _mockMapper = new Mock<IMapper>();

        // Primary Constructor kullanımına dikkat
        _handler = new GetAllAuditLogsQueryHandler(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WhenLogsExist_ShouldReturnMappedDtoList()
    {
        // --- 1. Arrange ---
        var logs = new List<AuditLogEntity>
        {
            new() { Id = 1, Action = "Create", EntityName = "Asset", Details = "Test 1" },
            new() { Id = 2, Action = "Update", EntityName = "Asset", Details = "Test 2" }
        };

        var dtos = new List<AuditLogResponseDto>
        {
            new() { Action = "Create", EntityName = "Asset", Details = "Test 1" },
            new() { Action = "Update", EntityName = "Asset", Details = "Test 2" }
        };

        _mockRepo.Setup(x => x.GetAllLogsAsync()).ReturnsAsync(logs);

        // Liste mapping taklidi
        _mockMapper.Setup(x => x.Map<IEnumerable<AuditLogResponseDto>>(logs))
                   .Returns(dtos);

        // --- 2. Act ---
        var result = await _handler.Handle(new GetAllAuditLogsQuery(), CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.First().Action.ShouldBe("Create");
        _mockRepo.Verify(x => x.GetAllLogsAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoLogsExist_ShouldReturnEmptyList()
    {
        // --- 1. Arrange ---
        var emptyLogs = new List<AuditLogEntity>();
        var emptyDtos = new List<AuditLogResponseDto>();

        _mockRepo.Setup(x => x.GetAllLogsAsync()).ReturnsAsync(emptyLogs);
        _mockMapper.Setup(x => x.Map<IEnumerable<AuditLogResponseDto>>(emptyLogs))
                   .Returns(emptyDtos);

        // --- 2. Act ---
        var result = await _handler.Handle(new GetAllAuditLogsQuery(), CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldBeEmpty();
        _mockRepo.Verify(x => x.GetAllLogsAsync(), Times.Once);
    }
}