namespace PayApp.Core.Enums
{
    public enum CsvProcessingIndex
    {
        FirstName=0,
        LastName,
        GrossIncome,
        SuperAnnuationRate,
        DatePeriod
    }

    public enum TaxRateProcessingIndex
    {
        BaseTax = 0,
        BaseRate,
        MinSalaryValue,
        MaxSalaryValue,
        StartDate
    }

    public enum TimeFrequency
    {
        Fortnightly=26,
        Monthly=12,
        Yearly=1
    }
}
