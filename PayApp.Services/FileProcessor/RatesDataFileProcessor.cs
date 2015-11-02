using System;
using System.IO;
using System.Linq;
using FluentValidation.Results;
using PayApp.Core.Enums;
using PayApp.Core.Extensions;
using PayApp.Core.Models;
using PayApp.Core.Validators;
using PayApp.Data.Rates;

namespace PayApp.Services.FileProcessor
{
    public class RatesDataFileProcessor : IFileProcessor
    {

        private readonly IRateDatasSource _taxRates;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taxRatesDb"></param>
        public RatesDataFileProcessor(IRateDatasSource taxRatesDb )
        {
            _taxRates = taxRatesDb;
            //_outputWriter = outputWriter;
        }

        /// <summary>
        /// Passing path of file to process and check if empty
        /// </summary>
        /// <param name="path"></param>
        public void Run(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return;
            }
            ProcessFile(path);
        }

        /// <summary>
        /// Read the input data file to process tax rates
        /// </summary>
        /// <param name="path"></param>
        public void ProcessFile(string path)
        {
            var lines = File.ReadLines(path).ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                
                ProcessLine(line);
                
            }
        }

        /// <summary>
        /// Parses the rate line from CSV to be added to TaxRateDB (in-memory)
        /// </summary>
        /// <param name="line"></param>
        public void ProcessLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] processLine = line.Split(',');

                if (processLine.Length > 0)
                {
                    var bracket = new TaxBracket
                    {
                        BaseTax = processLine[Convert.ToInt32(TaxRateProcessingIndex.BaseTax)].StringToDecimal(),

                        BaseRate = processLine[Convert.ToInt32(TaxRateProcessingIndex.BaseRate)].StringToDecimal(),

                        MinSalaryValue =
                            processLine[Convert.ToInt32(TaxRateProcessingIndex.MinSalaryValue)].StringToDecimal(),

                        MaxSalaryValue =
                            processLine[Convert.ToInt32(TaxRateProcessingIndex.MaxSalaryValue)].StringToDecimal(),

                        StartDate =
                            processLine[Convert.ToInt32(TaxRateProcessingIndex.StartDate)].ConvertStringtoDateTime()
                    };

                    RateValidator rateValidator = new RateValidator();

                    ValidationResult results = rateValidator.Validate(bracket);

                    if (results.IsValid)
                    {
                        _taxRates.AddTaxRate(bracket);
                    }
      
                }
            }
        }
    }
}
    
