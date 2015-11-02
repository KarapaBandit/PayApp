using System;
using System.Linq;
using PayApp.Core.Enums;
using PayApp.Core.Extensions;
using PayApp.Core.Models;
using PayApp.Data.Rates;

namespace PayApp.Services.Tax
{ 
    public class TaxCalculationService : ITaxService
    {
        private readonly IRateDatasSource _taxRates;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="taxRates"></param>
        public TaxCalculationService(IRateDatasSource taxRates)
        {
            _taxRates = taxRates;
        }

        /// <summary>
        /// Calculates the income tax on the income based on date and frequency
        /// </summary>
        /// <param name="date"></param>
        /// <param name="income"></param>
        /// <param name="timeFrequency"></param>
        /// <returns>decimal?</returns>
        public decimal? IncomeTaxCalculation(DateTime date, decimal income, TimeFrequency timeFrequency)
        {
            var taxBracket =  AllocatedTaxRate(date, income);
            if (taxBracket != null)
            {
                return (taxBracket.BaseTax + (income - taxBracket.MinSalaryValue.RoundValueToNearest100()) * taxBracket.BaseRate) / Convert.ToDecimal(timeFrequency);
            }

            return null;
        }

        /// <summary>
        /// Get the tax rate to apply to calculate taxes
        /// </summary>
        /// <param name="date"></param>
        /// <param name="income"></param>
        /// <returns>TaxBracket</returns>
        public TaxBracket AllocatedTaxRate(DateTime date, decimal income)
        {
           return  _taxRates.GetRates(date)
                .FirstOrDefault<TaxBracket>(entry => income >= entry.MinSalaryValue && income <= entry.MaxSalaryValue);
        }
    }
}
