using System;
using System.Text;
using PayApp.Core.Extensions;
using PayApp.Core.Models;
using PayApp.Core.Presentation.ViewModels;

namespace PayApp.Core.Presentation.Extensions
{
   public static class ExtensionFunctions
    {
        /// <summary>
        /// Concatentates the firstName and LastName to provide Customer FullName
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>string</returns>
        public static string GetFullName(this Customer customer)
       {
           return customer.FirstName + " " + customer.LastName;
       }

       /// <summary>
       /// Get the date range as string based on the customer payperiod. 
       /// </summary>
       /// <param name="period"></param>
       /// <param name="cust"></param>
       /// <returns>string</returns>
       public static string GetPayPeriodWithHypen(this PayPeriod period)
       {
            StringBuilder periodSb = new StringBuilder();

            periodSb.Append(period.Month.ToString(DateTimeExt.DateTimeFormat));
            periodSb.Append(" - ");
            periodSb.Append(System.DateTime.DaysInMonth(period.Month.Year, period.Month.Month) +
                " " + period.Month.ToString(DateTimeExt.DateTimeFormatExDays));

           return periodSb.ToString();
       }

        /// <summary>
        /// Return PayPeriod from the for the input model
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>PayPeriod</returns>
        public static PayPeriod GetPayPeriod(this ProcessingPayLineVm customer)
       {
           string[] dateTimes = customer.StartDateTime.Split('-');
           if (dateTimes.Length ==2)
           {
                return new PayPeriod
                {
                    Month = dateTimes[0].Trim().ConvertStringtoDateTime(),
                    Package = new SalaryPackage
                    {
                        SuperAnnuationRate = Convert.ToDecimal(customer.SuperAnnuationRate.Trim().TrimEnd('%'))/100.00m,
                        AnnualGrossSalary = Convert.ToDecimal(customer.AnnualGrossSalary)
                    }
                };
            }

           return new PayPeriod();
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payslip"></param>
        /// <returns></returns>
        public static string ToConsoleLineString(this PaySlipVm payslip)
       {
            //name, pay period, gross income, income tax, net income, super
           StringBuilder sb = new StringBuilder();
           sb.Append(payslip.CustomerFullName + ",");
           sb.Append(payslip.Period  + ",");
           sb.Append(payslip.GrossSalary + ",");
           sb.Append(payslip.IncomeTax + ",");
           sb.Append(payslip.NetIncome + ",");
           sb.Append(payslip.SuperAnnuation);

           return sb.ToString();
       }


    
    }
}
