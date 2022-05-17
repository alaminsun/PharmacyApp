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
            RuleFor(p => p.Category_Id)
                .NotEmpty().WithMessage("Select Category required.");

            RuleFor(p => p.Shelf_Id)
                .NotEmpty().WithMessage("Select Shelf required.");

            RuleFor(p => p.Medicine_Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            //RuleFor(p => p.Batch_No)
            //    .NotEmpty().WithMessage("BatchNo is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("BatchNo must not exceed 50 characters.");
            RuleFor(p => p.Buying_Price)
                      .NotEmpty()
                      .WithMessage("BuyingPrice is Required")
                      .GreaterThanOrEqualTo(1).WithMessage("BuyingPrice must be greater than 0");
            RuleFor(p => p.Selling_Price)
                      .NotEmpty()
                      .WithMessage("SellingPrice is Required")
                      .GreaterThanOrEqualTo(1).WithMessage("SellingPrice must be greater than 0");
            //RuleFor(p => p.Expiry_Date)
            //   .NotEmpty().WithMessage("Date is required.")
            //   .NotNull();


        }
    }
}
