using FluentValidation;
using PhramacyApp.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApp.Areas.Admin.Validators
{
    public class MenuViewModelValidator : AbstractValidator<MenuViewModel>
    {
        public MenuViewModelValidator()
        {
            RuleFor(p => p.Mh_Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(p => p.Area)
                .NotEmpty().WithMessage("Area is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(p => p.Mh_Seq).GreaterThanOrEqualTo(1).WithMessage("Sequence must be greater than 0");

            //RuleFor(p => p.Role_Id)
            //     .NotEmpty().WithMessage("Role is required.")
            //     .NotNull();




        }
    }
}
