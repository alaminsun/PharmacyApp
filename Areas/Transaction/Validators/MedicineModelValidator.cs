using FluentValidation;
using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Validators
{
    public class MedicineModelValidator : AbstractValidator<MedicineModel>
    {
        public MedicineModelValidator()
        {
            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Select Category required.");

            RuleFor(p => p.ShelfId)
                .NotEmpty().WithMessage("Select Shelf required.");

            RuleFor(p => p.MedicineName)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(p => p.BatchNo)
                .NotEmpty().WithMessage("BatchNo is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("BatchNo must not exceed 50 characters.");
            RuleFor(p => p.BuyingPrice)
                      .NotEmpty()
                      .WithMessage("BuyingPrice is Required")
                      .GreaterThanOrEqualTo(1).WithMessage("BuyingPrice must be greater than 0");
            RuleFor(p => p.SellingPrice)
                      .NotEmpty()
                      .WithMessage("SellingPrice is Required")
                      .GreaterThanOrEqualTo(1).WithMessage("SellingPrice must be greater than 0");
            RuleFor(p => p.ExpiryDate)
               .NotEmpty().WithMessage("Date is required.")
               .NotNull();


        }
    }
}
