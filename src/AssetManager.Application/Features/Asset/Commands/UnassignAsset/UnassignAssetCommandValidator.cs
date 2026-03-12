using FluentValidation;


namespace AssetManager.Application.Features.Asset.Commands.UnassignAsset
{
    public class UnassignAssetCommandValidator : AbstractValidator<UnassignAssetCommand>
    {
        public UnassignAssetCommandValidator()
        {
            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("A valid Asset ID is required for unassigning.");
        }
    }
}
