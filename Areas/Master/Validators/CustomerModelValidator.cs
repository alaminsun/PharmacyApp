using FluentValidation;
using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Validators
{
    public class CustomerModelValidator : AbstractValidator<CustomerModel>
    {
        public CustomerModelValidator()
        {
            RuleFor(p => p.Customer_Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            //RuleFor(p => p.Notes)
            //    .NotEmpty().WithMessage("Note is required.")
            //    .NotNull()
            //    .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(p => p.Email)
                //.NotEmpty()
                //.WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("A valid email address is required.");

            //RuleFor(p => p.Phone)
            //    .NotEmpty().WithMessage("Phone Number is required.")
            //    .NotNull()
            //    .MaximumLength(11).WithMessage("Number must not exceed 11 characters.");

            //RuleFor(p => p.Address)
            //    .NotEmpty().WithMessage("Adress is required.")
            //    .NotNull()
            //    .MaximumLength(200).WithMessage("Number must not exceed 200 characters.");



        }
    }
}
