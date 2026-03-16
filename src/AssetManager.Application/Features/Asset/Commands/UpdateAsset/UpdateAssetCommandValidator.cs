using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Asset.Commands.UpdateAsset
{
    public class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
    {
        public UpdateAssetCommandValidator()
        {
            // Id: Boş olamaz ve 0'dan büyük olmalı
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Asset ID is required.")
                .GreaterThan(0).WithMessage("Invalid Asset ID.");

            // Name: Boş olamaz, 2-200 karakter arası (Configuration ile uyumlu)
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

            // Price: Negatif olamaz (Entity'deki Value karşılığı)
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

            // Description: Max 500 karakter
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            // AssignedUserId: Opsiyoneldir, bu yüzden özel bir kurala (null değilse 0'dan büyük olmalı gibi) 
            // gerek yoksa boş bırakılabilir, ancak atanacaksa geçerli bir ID olması beklenir.
            RuleFor(x => x.AssignedUserId)
                .GreaterThan(0).When(x => x.AssignedUserId.HasValue)
                .WithMessage("Invalid User ID for assignment.");
        }
    }
}