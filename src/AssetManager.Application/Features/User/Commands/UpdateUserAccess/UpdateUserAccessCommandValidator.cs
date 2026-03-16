using FluentValidation;
using AssetManager.Domain.Enums;

namespace AssetManager.Application.Features.User.Commands.UpdateUserAccess;

public class UpdateUserAccessCommandValidator : AbstractValidator<UpdateUserAccessCommand>
{
    public UpdateUserAccessCommandValidator()
    {
        // UserId boş olamaz ve 0'dan büyük olmalı
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .GreaterThan(0).WithMessage("Invalid User ID.");

        // Rol boş olamaz ve tanımlı rollerimizden biri olmalı
        RuleFor(v => v.NewRole)
            .NotEmpty().WithMessage("Role is required.")
            .Must(BeAValidRole).WithMessage("Invalid role type. Use: Admin, Editor, or Guest.");

        // DepartmentId zorunlu değil (null olabilir) ama dolu gelirse 0'dan büyük olmalı
        RuleFor(v => v.DepartmentId)
            .GreaterThan(0)
            .When(v => v.DepartmentId.HasValue)
            .WithMessage("Invalid Department ID.");
    }

    // Rolün Enum içinde olup olmadığını kontrol eden yardımcı metot
    private bool BeAValidRole(string role)
    {
        return Enum.TryParse<Roles>(role, true, out _);
    }
}