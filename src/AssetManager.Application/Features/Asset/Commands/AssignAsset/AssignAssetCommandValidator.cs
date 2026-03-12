using FluentValidation;


namespace AssetManager.Application.Features.Asset.Commands.AssignAsset
{
    public class AssignAssetCommandValidator : AbstractValidator<AssignAssetCommand>
    {
        public AssignAssetCommandValidator()
        {
            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("Valid Asset ID is required.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Valid User ID is required.");
        }
    }
}
