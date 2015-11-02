using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using PayApp.Core.Models;
using PayApp.Data.Rates;
using PayApp.Test.Helpers;
using Xunit;

namespace PayApp.Test
{
    public class PayAppDataTests
    {

        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData(2013,3,1)]
        private void Get_Tax_Rates_Test(int year, int month, int day, TaxRateDb sut)
        {

            // Assign
            DateTime date = new DateTime(year,month,day);
            TaxBracket tb = new TaxBracket
            {
                BaseRate = 0.19m,
                BaseTax = 0M,
                MinSalaryValue = 18201,
                MaxSalaryValue = 37000,
                StartDate = new DateTime(2012, 7, 1)
            };
            sut.AddTaxRate(TestStubs.TaxBrackets()[0]);
            sut.AddTaxRate(tb);

            //Assign the expected
            List<TaxBracket> expected = new List<TaxBracket>();
            expected.Add(tb);
            expected.Add(TestStubs.TaxBrackets()[0]);

            // Adding rate outside range to test for 
            sut.AddTaxRate(                   
                   new TaxBracket
                   {
                       BaseRate = 0.37m,
                       BaseTax = 17547m,
                       MinSalaryValue = 80001,
                       MaxSalaryValue = 180000,
                       StartDate = new DateTime(2013, 7, 1)
                   });


            //Act
            List<TaxBracket> actual = sut.GetRates(date).ToList();
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(expected, actual);

            // Assert tax to verify it works
            Assert.True(result.AreEqual);

        }

    }
}
