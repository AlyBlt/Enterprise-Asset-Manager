using AssetManager.Application.Features.Department.Commands.CreateDepartment;
using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Domain.Entities;
using AutoMapper;
using Moq;
using Shouldly;

namespace AssetManager.UnitTests.Features.Departments;

public class CreateDepartmentCommandHandlerTests
{
    private readonly Mock<IDepartmentRepository> _mockDeptRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAuditLogService> _mockAuditLogService;
    private readonly CreateDepartmentCommandHandler _handler;

    public CreateDepartmentCommandHandlerTests()
    {
        _mockDeptRepo = new Mock<IDepartmentRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockAuditLogService = new Mock<IAuditLogService>();

        _handler = new CreateDepartmentCommandHandler(
            _mockDeptRepo.Object,
            _mockMapper.Object,
            _mockAuditLogService.Object);
    }

    [Fact]
    public async Task Handle_ValidDepartment_ShouldReturnTrueAndLogAudit()
    {
        // --- 1. Arrange ---
        // Record olduğu için constructor kullanımı: Name, Description
        var command = new CreateDepartmentCommand("IT Department", "Information Technology Support");

        var departmentEntity = new DepartmentEntity
        {
            Id = 1,
            Name = command.Name,
            Description = command.Description
        };

        // Mapper Kurulumu
        _mockMapper.Setup(m => m.Map<DepartmentEntity>(It.IsAny<CreateDepartmentCommand>()))
                   .Returns(departmentEntity);

        // Repo Kurulumu (Kayıt başarılı dönsün)
        _mockDeptRepo.Setup(r => r.SaveChangesAsync())
                    .ReturnsAsync(1);

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldBeTrue();

        // Repository'ye gerçekten ekleme yapıldı mı?
        _mockDeptRepo.Verify(r => r.AddAsync(It.IsAny<DepartmentEntity>()), Times.Once);

        // Audit Log atıldı mı? (Sadece result true olduğunda atılmalı)
        _mockAuditLogService.Verify(l => l.LogAsync(
            "Create",
            "Department",
            departmentEntity.Name,
            It.Is<string>(s => s.Contains(departmentEntity.Name))), Times.Once);
    }

    [Fact]
    public async Task Handle_DatabaseFailure_ShouldReturnFalseAndNotLogAudit()
    {
        // --- 1. Arrange ---
        var command = new CreateDepartmentCommand("HR", "Human Resources");

        _mockMapper.Setup(m => m.Map<DepartmentEntity>(It.IsAny<CreateDepartmentCommand>()))
                   .Returns(new DepartmentEntity { Name = "HR" });

        // Veritabanı kaydı başarısız (0 satır etkilendi) dönsün
        _mockDeptRepo.Setup(r => r.SaveChangesAsync())
                    .ReturnsAsync(0);

        // --- 2. Act ---
        var result = await _handler.Handle(command, CancellationToken.None);

        // --- 3. Assert ---
        result.ShouldBeFalse();

        // Kayıt başarısız olduğu için LogAsync ASLA çağrılmamalı
        _mockAuditLogService.Verify(l => l.LogAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Never);
    }
}