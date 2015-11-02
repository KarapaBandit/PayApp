using System;
using PayApp.Core.Enums;
using PayApp.Core.Models;

namespace PayApp.Services.Tax
{
    public interface ITaxService
    {
        decimal? IncomeTaxCalculation(DateTime date, decimal income, TimeFrequency timeFrequency);
        TaxBracket AllocatedTaxRate(DateTime date, decimal income);
    }
}
