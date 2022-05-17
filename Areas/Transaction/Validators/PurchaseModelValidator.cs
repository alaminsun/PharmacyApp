using FluentValidation;
using PhramacyApp.Areas.Master.Models;
using PhramacyApp.Areas.Transaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Transaction.Validators
{
    public class PurchaseModelValidator : AbstractValidator<PurchaseMasterModel>
    {
        public PurchaseModelValidator()
        {
            RuleFor(p => p.Supplier_Id)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            //RuleFor(p => p.SupplierId).WithMessage(required);
            //RuleFor(p => p.Notes)
            //    .NotEmpty().WithMessage("Note is required.")
            //    .NotNull()
            //    .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(p => p.Purchase_Date)
                .NotEmpty()
                .WithMessage("Purchase Date is required.");

            //RuleFor(p => p.DetailList[0].Quantity)
            // .NotEmpty().WithMessage("Quantity can't be empty.")
            // .NotNull().WithMessage("Quantity can't be empty.")
            // .GreaterThan(1).WithMessage("Quantity must be greater than 0.");

            RuleFor(p => p.DetailList[0].Quantity).InclusiveBetween(18, 50);


            RuleFor(p => p.Invoice_No)
                .NotEmpty().WithMessage("InvoiceNo is required.")
                .NotNull()
                .MaximumLength(11).WithMessage("InvoiceNo must not exceed 11 characters.");


            //RuleFor(p => p.Address)
            //    .NotEmpty().WithMessage("Adress is required.")
            //    .NotNull()
            //    .MaximumLength(200).WithMessage("Number must not exceed 200 characters.");



        }
    }
}
