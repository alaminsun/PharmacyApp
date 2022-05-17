using FluentValidation;
using PhramacyApp.Areas.Admin.Models;


namespace PharmaApp.Areas.Admin.Validators
{

    public class SubMenuViewModelValidator : AbstractValidator<SubMenuViewModel>
    {
        public SubMenuViewModelValidator()
        {
            RuleFor(p => p.Sm_Name)
                .NotEmpty().WithMessage("Sub Menu is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(p => p.URL)
                .NotEmpty().WithMessage("URL is required.")
                .NotNull();

            RuleFor(p => p.Sm_Seq)
                .NotEmpty()
                .WithMessage("Sequence is Required")
                .GreaterThanOrEqualTo(1).WithMessage("Sequence must be greater than 0");

            RuleFor(p => p.Mh_Id)
                .NotEmpty()
               .WithMessage("Menu Head is Required");
               //.GreaterThanOrEqualTo(1)
               //.WithMessage("Menu Head is Required");
            //.NotEmpty().WithMessage("Menu Head can't be empty.")
            //.NotNull();
            //.GreaterThan(1).WithMessage("Menu Head is Required");

            //RuleFor(p => p.Mh_Id)
            //     .NotEmpty().WithMessage("Menu Head is required.")
            //     .NotNull();




        }
    }
}
