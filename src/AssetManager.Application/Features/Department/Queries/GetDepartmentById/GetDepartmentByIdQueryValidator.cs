using AssetManager.Application.Features.Asset.Queries.GetAssetById;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Department.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryValidator : AbstractValidator<GetDepartmentByIdQuery>
    {
        public GetDepartmentByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Department ID must be greater than zero.");
        }
    }
}
