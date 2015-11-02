using System;
using FluentValidation.Results;
using PayApp.Core.Models;
using PayApp.Core.Validators;
using PayApp.Test.Helpers;
using Xunit;

namespace PayApp.Test
{
    public class PayAppCoreTests
    {

        [Fact]
        void SalaryPackageValidate_Pass_Test()
        {
            //Assign
            SalaryPackage sp = new SalaryPackage
            {
                AnnualGrossSalary = 60050,
                SuperAnnuationRate = 9
            };

            SalaryPackageValidator spv = new SalaryPackageValidator();
            
            //Act
            ValidationResult results   = spv.Validate(sp);

            //Assert
            Assert.True(results.IsValid);
        }


        [Fact]
        void SalaryPackageValidate_Fail_Pass_Test()
        {
            //Assign
            SalaryPackage sp = new SalaryPackage
            {
                AnnualGrossSalary = 60050
            };

            SalaryPackageValidator spv = new SalaryPackageValidator();

            //Act
            ValidationResult results = spv.Validate(sp);

            //Assert
            Assert.False(results.IsValid);
        }

        [Fact]
        void PayPeriodValidate_Pass_Test()
        {
            //Assign
            SalaryPackage sp = new SalaryPackage
            {
                AnnualGrossSalary = 60050,
                SuperAnnuationRate = 9
            };

            PayPeriod pp = new PayPeriod
            {
                Package = sp,
                Month = DateTime.Now
            };

            PayPeriodValidator ppv = new PayPeriodValidator();

            //Act
            ValidationResult results = ppv.Validate(pp);

            //Assert
            Assert.True(results.IsValid);
        }


        [Fact]
        void PayPeriodValidate_Fail_Pass_Test()
        {
            //Assign
            SalaryPackage sp = new SalaryPackage
            {
                AnnualGrossSalary = 60050,
                SuperAnnuationRate = 9
            };

            PayPeriod pp = new PayPeriod
            {
                Package = sp
            };

            PayPeriodValidator ppv = new PayPeriodValidator();

            //Act
            ValidationResult results = ppv.Validate(pp);

            //Assert
            Assert.False(results.IsValid);
        }


        [Fact]
        void CustomerValidate_Pass_Test()
        {
            //Assign
            Customer cus = TestStubs.GetCustomer();

            CustomerValidator cv = new CustomerValidator();

            //Act
            ValidationResult results = cv.Validate(cus);

            //Assert
            Assert.True(results.IsValid);
        }


        [Fact]
        void CustomerValidate_Fail_Pass_Test()
        {
            //Assign
            Customer cus = TestStubs.GetCustomer();
            cus.LastName = String.Empty;

            CustomerValidator cv = new CustomerValidator();

            //Act
            ValidationResult results = cv.Validate(cus);

            //Assert
            Assert.False(results.IsValid);

        }

    }
}
