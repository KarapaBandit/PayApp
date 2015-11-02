using AutoMapper;
using PayApp.Core.Models;
using PayApp.Core.Presentation.ViewModels;

namespace PayApp.Core.Presentation.Extensions
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// AutoMapper mapping static builder
        /// </summary>
        public static void Build()
        {
            // Mapping the ProcessPayLine to Customer for processing downstream
            Mapper.CreateMap<ProcessingPayLineVm, Customer>()
                .ForMember(des => des.PayPeriod, opt => opt.MapFrom(src => src.GetPayPeriod()));
        }
    }
}
