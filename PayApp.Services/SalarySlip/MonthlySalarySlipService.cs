using System;
using PayApp.Core.Enums;
using PayApp.Core.Extensions;
using PayApp.Core.Models;
using PayApp.Core.Presentation.Extensions;
using PayApp.Core.Presentation.ViewModels;
using PayApp.Services.Tax;

namespace PayApp.Services.SalarySlip
{
    public class MonthlySalarySlipService : ISalarySlipService
    {

        private readonly ITaxService _taxService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taxService"></param>
        public MonthlySalarySlipService(ITaxService taxService)
        {
            _taxService = taxService;
        }

        /// <summary>
        /// Generate the PaySlip for a customer to be outputted
        /// </summary>
        /// <param name="processCust"></param>
        /// <param name="frequency"></param>
        /// <returns>PaySlipVm</returns>
        public PaySlipVm GenerateSalarySlip(Customer processCust, TimeFrequency frequency)
        {

            if (processCust.PayPeriod != null)
            {
                //Gross Income
                var grossIncome = processCust.PayPeriod.Package.AnnualGrossSalary / Convert.ToDecimal(frequency);

                //Income Tax
                decimal? incomeTax = _taxService.IncomeTaxCalculation(processCust.PayPeriod.Month, processCust.PayPeriod.Package.AnnualGrossSalary,
                    frequency);

                if (!incomeTax.HasValue)  return null;

                //Net Income
                var netIncome = grossIncome - incomeTax;

                //SuperAnnuation Rate
                var superRate = processCust.PayPeriod.Package.SuperAnnuationRate;
                var superAnnuation = grossIncome * superRate;

                // Generate PaySlipVm for the view
                return new PaySlipVm
                {
                    GrossSalary = "" + grossIncome.RoundToNearestWhole(),
                    CustomerFullName = processCust.GetFullName(),
                    IncomeTax = "" + incomeTax.Value.RoundToNearestWhole(),
                    NetIncome = "" + netIncome.Value.RoundToNearestWhole(),
                    Period = processCust.PayPeriod.GetPayPeriodWithHypen(),
                    SuperAnnuation = "" + superAnnuation.RoundToNearestWhole()
                };
            }

            return null;

        }
    }
}
