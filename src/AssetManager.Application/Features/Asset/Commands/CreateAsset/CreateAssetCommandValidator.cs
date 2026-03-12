using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Commands.CreateAsset
{
    public class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand>
    {
        public CreateAssetCommandValidator()
        {
            // Name: Boş olamaz, 2-200 karakter arası
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Asset name is required.")
                .Length(2, 200).WithMessage("Asset name must be between 2 and 200 characters.");

            // Category: Boş olamaz, max 50 karakter
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(50).WithMessage("Category cannot exceed 50 characters.");

            // SerialNumber: Boş olamaz, max 100 karakter
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("Serial number is required.")
                .MaximumLength(100).WithMessage("Serial number cannot exceed 100 characters.");

            // Price: Negatif olamaz
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

            // Description: Max 500 karakter (Boş bırakılabilir)
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
