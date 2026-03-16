using MediatR;

namespace AssetManager.Application.Features.Department.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(int Id, string Name, string Description) : IRequest<bool>;