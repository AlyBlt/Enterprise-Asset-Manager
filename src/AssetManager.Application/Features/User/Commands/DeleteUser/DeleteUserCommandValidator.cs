using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.")
                .GreaterThan(0).WithMessage("User ID must be greater than zero.");
        }
    }
}
