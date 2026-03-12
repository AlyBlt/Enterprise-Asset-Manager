using FluentValidation;


namespace AssetManager.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID must be greater than zero.");
        }
    }
}
