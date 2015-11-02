using System;

namespace PayApp.Core.Models
{
    public class TaxBracket 
    {
       
        public Decimal BaseTax { get; set; }

        public Decimal BaseRate { get; set; }

        public Decimal MinSalaryValue { get; set; }

        public Decimal MaxSalaryValue { get; set; }

        public DateTime StartDate { get; set; }

    }
}
