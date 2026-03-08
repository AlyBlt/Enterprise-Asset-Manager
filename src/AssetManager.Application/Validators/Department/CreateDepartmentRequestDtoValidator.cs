using AssetManager.Application.DTOs.Department;
using FluentValidation;

namespace AssetManager.Application.Validators.Department;

public class CreateDepartmentRequestDtoValidator : AbstractValidator<CreateDepartmentRequestDto>
{
    public CreateDepartmentRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MinimumLength(2).WithMessage("Department name must be at least 2 characters.")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}