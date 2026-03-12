using FluentValidation;


namespace AssetManager.Application.Features.Asset.Queries.GetAssetById
{
    public class GetAssetByIdQueryValidator : AbstractValidator<GetAssetByIdQuery>
    {
        public GetAssetByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Asset ID must be greater than zero.");
        }
    }
}
