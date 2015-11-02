using PayApp.Core.Enums;
using PayApp.Core.Models;
using PayApp.Core.Presentation.ViewModels;

namespace PayApp.Services.SalarySlip
{
    public interface ISalarySlipService
    {
        PaySlipVm GenerateSalarySlip(Customer processCust, TimeFrequency frequency);

       
    }
}
