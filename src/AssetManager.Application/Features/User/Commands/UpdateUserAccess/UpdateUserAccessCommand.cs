using MediatR;

namespace AssetManager.Application.Features.User.Commands.UpdateUserAccess;

public record UpdateUserAccessCommand(
    int UserId,
    string NewRole,
    int? DepartmentId) : IRequest<bool>;