using FluentValidation;
using PayApp.Core.Models;

namespace PayApp.Core.Validators
{
    public class RateValidator : AbstractValidator<TaxBracket>
    {
        /// <summary>
        /// TaxRate validator
        /// </summary>
        public RateValidator()
        {
            RuleFor(rate => rate.BaseTax).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(rate => rate.BaseRate).NotEmpty().WithMessage("{PropertyName} is required");

        }
    }
}
