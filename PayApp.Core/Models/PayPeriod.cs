using System;

namespace PayApp.Core.Models
{
    public class PayPeriod 
    {
        public SalaryPackage Package { get; set; }
        public DateTime Month { get; set; }
    }
}
