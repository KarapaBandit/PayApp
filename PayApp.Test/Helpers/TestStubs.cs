using System;
using System.Collections.Generic;
using PayApp.Core.Models;
using PayApp.Core.Presentation.ViewModels;

namespace PayApp.Test.Helpers
{
    public static class TestStubs
    {
        /// <summary>
        /// List of TaxBracket Stub
        /// </summary>
        /// <returns>List of TaxBracket</returns>
        public static List<TaxBracket> TaxBrackets()
        {
            return new List<TaxBracket>
            {
                new TaxBracket
                {
                    BaseRate = 0.325M,
                    BaseTax = 3572M,
                    MinSalaryValue = 37001,
                    MaxSalaryValue = 80000,
                    StartDate = new DateTime(2012, 7, 1)
                }
            };
        }

        /// <summary>
        /// Customer Stub
        /// </summary>
        /// <returns>Customer</returns>
        public static Customer GetCustomer()
        {
           return new Customer
            {
                FirstName = "DavidTest",
                LastName = "RuddTest",
                PayPeriod =
                        new PayPeriod
                        {
                            Month = new DateTime(2013, 3, 1),
                            Package = new SalaryPackage
                            {
                                SuperAnnuationRate = 0.10m,
                                AnnualGrossSalary = 120000

                            }
                        }
            };
        }

        /// <summary>
        /// PaySlipVm Stub
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="payPeriod"></param>
        /// <returns>PaySlipVm</returns>
        public static PaySlipVm GetPaySlipVm(string fullName, string payPeriod)
        {
            return  new PaySlipVm
            {
                GrossSalary = "10000",
                CustomerFullName = fullName,
                IncomeTax = "2696",
                NetIncome = "7304",
                Period = payPeriod,
                SuperAnnuation = "1000"
            };
        }
    }
}
