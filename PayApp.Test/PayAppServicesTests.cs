using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using Moq;
using PayApp.Core.Enums;
using PayApp.Core.Models;
using PayApp.Core.Presentation.Extensions;
using PayApp.Core.Presentation.ViewModels;
using PayApp.Data.Rates;
using PayApp.Services.FileProcessor;
using PayApp.Services.OutputWriter;
using PayApp.Services.SalarySlip;
using PayApp.Services.Tax;
using PayApp.Test.Helpers;
using Ploeh.AutoFixture.Xunit2;
using Xunit;


namespace PayApp.Test
{
    public class PayAppServicesTests
    {

        #region TaxService Tests

        /// <param name="income"></param>
        /// <param name="timeFrequency"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="taxRates"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData(60050, TimeFrequency.Monthly, 2013, 3, 1)]
        void TaxServices_Calculate_Tax(decimal income, TimeFrequency timeFrequency, int year, int month, int day,
            [Frozen] Mock<IRateDatasSource> taxRates, TaxCalculationService sut)
        {

            // Assign
            DateTime date = (new DateTime(year, month, day));
            taxRates.Setup(x => x.GetRates(date)).Returns(TestStubs.TaxBrackets());
            decimal expected = 921.9375m;

            //Act
            var actual = sut.IncomeTaxCalculation(date, income, timeFrequency);
            
            // Assert getRates is called.
            taxRates.Verify(x => x.GetRates(It.IsAny<DateTime>()), Times.Once());

            // Assert tax to verify it works
            Assert.Equal(expected, actual);

        }

  
        /// <param name="income"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="taxRates"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData(60050, 2013, 3, 1)]
        void TaxService_TaxBracket_Pass(decimal income, int year, int month, int day,
            [Frozen] Mock<IRateDatasSource> taxRates, TaxCalculationService sut)
        {
            //Assign
            DateTime date = (new DateTime(year, month, day));
            taxRates.Setup(x => x.GetRates(date)).Returns(TestStubs.TaxBrackets());

            TaxBracket expected = TestStubs.TaxBrackets()[0];
    
            //Act
            TaxBracket actual = sut.AllocatedTaxRate(date, income);
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(expected, actual);

            // Assert getRates is called.
            taxRates.Verify(x => x.GetRates(It.IsAny<DateTime>()), Times.Once());

            // Assert tax to verify if works
            Assert.True(result.AreEqual);
        }

    
        /// <param name="income"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="taxRates"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData(60050, 2012, 3, 1)]
        void TaxService_TaxBracket_Failed_Pass(decimal income, int year, int month, int day,
        [Frozen] Mock<IRateDatasSource> taxRates, TaxCalculationService sut)
        {

            //Assign
            DateTime date = (new DateTime(year, month, day));
            taxRates.Setup(x => x.GetRates(date)).Returns(new List<TaxBracket>());

            //Act
            TaxBracket actual = sut.AllocatedTaxRate(date, income);

            // Assert getRates is called.
            taxRates.Verify(x => x.GetRates(It.IsAny<DateTime>()), Times.Once());

            // Assert tax to verify it works
            Assert.Null(actual);
        }

        #endregion


        #region SalarySlip Tests


        /// <param name="frequency"></param>
        /// <param name="taxService"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData(TimeFrequency.Monthly)]
        void SalarySlipService_GenerateSalarySlip(TimeFrequency frequency, [Frozen] Mock<ITaxService> taxService, MonthlySalarySlipService sut)
        {

            // Assign
            Customer cust = TestStubs.GetCustomer();
            PaySlipVm expected = TestStubs.GetPaySlipVm(cust.GetFullName(), cust.PayPeriod.GetPayPeriodWithHypen());
            taxService.Setup(x => x.IncomeTaxCalculation(cust.PayPeriod.Month, cust.PayPeriod.Package.AnnualGrossSalary, frequency)).Returns(Convert.ToDecimal(expected.IncomeTax));
            

            // Act
            PaySlipVm actual = sut.GenerateSalarySlip(cust, frequency);
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(expected, actual);

            // Assert income tax calculation is called.
            taxService.Verify(x => x.IncomeTaxCalculation(It.IsAny<DateTime>(), It.IsAny<Decimal>(), It.IsAny<TimeFrequency>()), Times.Once());

            //Assertions for Objects
            Assert.True(result.AreEqual);
        }

        #endregion


        #region FileProcess Tests


        /// <param name="line"></param>
        /// <param name="outputWriter"></param>
        /// <param name="salarySlipService"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData("DavidTest,RuddTest,120000,10%,01 March 2013-31 March 2013")]
        void SalaryFileProcessor_Successful_Test(string line,  [Frozen] Mock<IOutputWriter> outputWriter, [Frozen] Mock<ISalarySlipService> salarySlipService, SalaryDataFileProcessor sut )
        {
            //Assign
            Customer cust = TestStubs.GetCustomer();
            PaySlipVm paySlipVm = TestStubs.GetPaySlipVm(cust.GetFullName(), cust.PayPeriod.GetPayPeriodWithHypen());
            salarySlipService.Setup(x => x.GenerateSalarySlip(cust, TimeFrequency.Monthly)).Returns(paySlipVm);
            AutoMapperExtensions.Build();

            //Act
            sut.ProcessLine(line);

            //Assert income tax calculation is called.
            salarySlipService.Verify(x =>  x.GenerateSalarySlip(It.IsAny<Customer>(), It.IsAny<TimeFrequency>()), Times.Once());
            outputWriter.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once());

        }


        /// <param name="line"></param>
        /// <param name="taxRates"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData("3572,0.325,37001,80000,01 July 2012")]
        void RatesFileProcessor_AddTaxRate_Test(string line,  [Frozen] Mock<IRateDatasSource> taxRates, RatesDataFileProcessor sut)
        {
            //Assign
            TaxBracket tb = TestStubs.TaxBrackets()[0];
            taxRates.Setup(x => x.AddTaxRate(tb));
            

            //Act
            sut.ProcessLine(line);

            //Assert income tax calculation is called.
            taxRates.Verify(x => x.AddTaxRate(It.IsAny<TaxBracket>()), Times.Once());
        }

        /// <param name="path"></param>
        /// <param name="outputWriter"></param>
        /// <param name="salarySlipService"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData("PayApp.Test/TestData/SuccessSalaryReadFile.csv")]
        void SalaryService_Process_File_SuccessfulRead_Test(string path, [Frozen] Mock<IOutputWriter> outputWriter, [Frozen] Mock<ISalarySlipService> salarySlipService, SalaryDataFileProcessor sut)
        {

            //Assign
            Customer cust = TestStubs.GetCustomer();
            PaySlipVm paySlipVm = TestStubs.GetPaySlipVm(cust.GetFullName(), cust.PayPeriod.GetPayPeriodWithHypen());
            salarySlipService.Setup(x => x.GenerateSalarySlip(cust, TimeFrequency.Monthly)).Returns(paySlipVm);
            AutoMapperExtensions.Build();

            //Act
            sut.ProcessFile(HelperMethods.GetTestDataFolder("" + path));

            //Assert output writer is called.
            salarySlipService.Verify(x => x.GenerateSalarySlip(It.IsAny<Customer>(), It.IsAny<TimeFrequency>()), Times.AtLeastOnce);
            outputWriter.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);

        }


        /// <param name="path"></param>
        /// <param name="outputWriter"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData("PayApp.Test/TestData/FailedSalaryReadFile.csv")]
        void SalaryService_Process_File_Exception_Test(string path, [Frozen] Mock<IOutputWriter> outputWriter,  SalaryDataFileProcessor sut)
        {
            //Act
            sut.ProcessFile(HelperMethods.GetTestDataFolder(""+path));

            //Assert output writer is called.
            outputWriter.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(3));

        }

        /// <param name="path"></param>
        /// <param name="outputWriter"></param>
        /// <param name="sut"></param>
        [Theory]
        [InlineAutoMoqData("PayApp.Test/TestData/EmptySalaryReadFile.csv")]
        void SalaryService_Process_File_Empty_Test(string path, [Frozen] Mock<IOutputWriter> outputWriter, SalaryDataFileProcessor sut)
        {
            //Act
            sut.ProcessFile(HelperMethods.GetTestDataFolder("" + path));

            //Assert output writer is called.
            outputWriter.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
        }


        #endregion
      

    }
}
