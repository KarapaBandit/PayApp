using System;
using System.Collections.Generic;
using PayApp.Core.Models;

namespace PayApp.Data.Rates
{
    public interface IRateDatasSource
    {
        IEnumerable<TaxBracket> GetRates(DateTime startDate);
        void AddTaxRate(TaxBracket rate);
    }
}
