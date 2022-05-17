using FluentValidation;
using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Validators
{
    public class CategoryModelValidator : AbstractValidator<CategoryModel>
    {
        public CategoryModelValidator()
        {
            RuleFor(p => p.Category_Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        }
    }
}
