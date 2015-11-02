using FluentValidation;
using PayApp.Core.Models;

namespace PayApp.Core.Validators
{
    public class PayPeriodValidator : AbstractValidator<PayPeriod>
    {
        /// <summary>
        /// Validation Rules for PayPeriod incorporating SalaryPackage
        /// </summary>
        public PayPeriodValidator()
        {
            RuleFor(payPeriod => payPeriod.Month).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(payPeriod => payPeriod.Package).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(payPeriod => payPeriod.Package).SetValidator(new SalaryPackageValidator());
        }

          
    }
}
