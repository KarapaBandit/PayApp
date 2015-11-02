using FluentValidation;
using PayApp.Core.Models;

namespace PayApp.Core.Validators
{
    public class SalaryPackageValidator : AbstractValidator<SalaryPackage>
    {
        /// <summary>
        /// Salary Package Validator
        /// </summary>
        public SalaryPackageValidator()
        {
            RuleFor(salaryPackage => salaryPackage.AnnualGrossSalary).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(salaryPackage => salaryPackage.SuperAnnuationRate).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
