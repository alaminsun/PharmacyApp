using FluentValidation;
using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Validators
{
    public class ShelfModelValidator : AbstractValidator<ShelfModel>
    {
        public ShelfModelValidator()
        {
            RuleFor(p => p.ShelfName)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(p => p.ShelfNumber)
                .NotEmpty().WithMessage("Area is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        }
    }
}
