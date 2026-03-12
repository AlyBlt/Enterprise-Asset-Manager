using FluentValidation;


namespace AssetManager.Application.Features.Asset.Commands.DeleteAsset
{
    public class DeleteAssetCommandValidator : AbstractValidator<DeleteAssetCommand>
    {
        public DeleteAssetCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Asset ID is required.")
                .GreaterThan(0).WithMessage("Asset ID must be greater than zero.");
        }
    }
}
