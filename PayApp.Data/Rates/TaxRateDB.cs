using System;
using System.Collections.Generic;
using System.Linq;
using PayApp.Core.Extensions;
using PayApp.Core.Models;

namespace PayApp.Data.Rates
{

   
    /// <summary>
    /// TaxRateDb please note this class does not inherit from DBContext and/or use EF 
    /// </summary>
    public class TaxRateDb :  IRateDatasSource
    {

        // Can be replaced with a DBcontent 
        public List<TaxBracket> TaxRatesCollection;

        /// <summary>
        /// Constructor which takes an input
        /// </summary>
        /// <param name="taxrates"></param>
        public TaxRateDb(List<TaxBracket> taxrates)
        {
            TaxRatesCollection = taxrates ?? new List<TaxBracket>();
        }

        /// <summary>
        /// Get Rates that are associated with the date range provided
        /// </summary>
        /// <param name="date"></param>
        /// <returns>IEnumerable of  TaxBracket</returns>
        public IEnumerable<TaxBracket> GetRates(DateTime date)
        {
            return from rate in TaxRatesCollection
                   where date.WithinDateTimeRange(rate.StartDate, rate.StartDate.AddYears(1))
                   orderby rate.MinSalaryValue ascending
                   select rate;
        }

        /// <summary>
        /// Add Rates to the collection
        /// </summary>
        /// <param name="rate"></param>
        public void AddTaxRate(TaxBracket rate)
        {
            TaxRatesCollection.Add(rate);
        }
    }
}
