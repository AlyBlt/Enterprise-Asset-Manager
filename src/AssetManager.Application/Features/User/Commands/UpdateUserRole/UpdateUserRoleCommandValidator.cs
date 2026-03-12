using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.NewRole).NotEmpty().WithMessage("Role name is required.");
        }
    }
}
