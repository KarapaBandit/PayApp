using KellermanSoftware.CompareNetObjects;
using PayApp.Core.Models;
using PayApp.Core.Presentation.Extensions;
using PayApp.Core.Presentation.ViewModels;
using PayApp.Test.Helpers;
using Xunit;

namespace PayApp.Test
{
    public class PayAppCorePresentationTests
    {

        [Fact]
        void Customer_Extension_Full_Name_Test()
        {
            //Assign
            Customer cust = TestStubs.GetCustomer();

            //Act
            string actual = cust.GetFullName();

            //Assert
            Assert.Equal("DavidTest RuddTest", actual);
        }

   
        [Fact]
        void PayPeriod_GetPayPeriodWithHypen_Test()
        {
            //Assign
            Customer cust = TestStubs.GetCustomer();

            //Act
            string actual = cust.PayPeriod.GetPayPeriodWithHypen();

            //Assert
            Assert.Equal("01 March 2013 - 31 March 2013", actual);
        }


        [Fact]
        void PayPeriod_GetPayPeriod_Test()
        {
            //Assign
            ProcessingPayLineVm ppl = new ProcessingPayLineVm
            {
                AnnualGrossSalary = "120000",
                FirstName = "DavidTest",
                LastName = "RuddTest",
                SuperAnnuationRate = "10%",
                StartDateTime = "01 March 2013-31 March 2013",

            };
            Customer cust = TestStubs.GetCustomer();

            //Act
            PayPeriod actual = ppl.GetPayPeriod();
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(cust.PayPeriod, actual);

            //Assert
            Assert.True(result.AreEqual);
        }


        [Fact]
        void PaySlipVm_ToConsoleLineString_Test()
        {
            //Assign
            PaySlipVm psvm = TestStubs.GetPaySlipVm("DavidTest RuddTest", "01 March 2013 – 31 March 2013");
            string expected = "DavidTest RuddTest,01 March 2013 – 31 March 2013,10000,2696,7304,1000";
           
            //Act
            string actual = psvm.ToConsoleLineString();

            //Assert
            Assert.Equal(expected, actual);
        }


    }
}
