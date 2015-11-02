using FluentValidation;
using PayApp.Core.Models;

namespace PayApp.Core.Validators
{
   public class CustomerValidator : AbstractValidator<Customer>
   {
        /// <summary>
        /// Validation Rules for Customer including PayPeriod
        /// </summary>
        public CustomerValidator()
       {
           RuleFor(customer => customer.FirstName).NotEmpty().WithMessage("{PropertyName} is required");

           RuleFor(customer => customer.LastName).NotEmpty().WithMessage("{PropertyName} is required");

           //RuleFor(customer => customer.PayPeriods)
           //    .Cascade(CascadeMode.StopOnFirstFailure)
           //    .NotNull()
           //    .Must(payPeriods => payPeriods.Count > 0)
           //    .WithMessage("{PropertyName} must be greater than '0')");

           RuleFor(customer => customer.PayPeriod).SetValidator(new PayPeriodValidator());
       }  
   }
}
