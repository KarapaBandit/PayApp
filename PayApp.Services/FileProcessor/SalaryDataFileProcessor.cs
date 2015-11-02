using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using PayApp.Core.Enums;
using PayApp.Core.Models;
using PayApp.Core.Presentation.Extensions;
using PayApp.Core.Presentation.ViewModels;
using PayApp.Core.Validators;
using PayApp.Services.OutputWriter;
using PayApp.Services.SalarySlip;

namespace PayApp.Services.FileProcessor
{
    public class SalaryDataFileProcessor : IFileProcessor
    {

        private readonly IOutputWriter _outputWriter;
        private readonly ISalarySlipService _salarySlipService;
        private List<string> _failedProcessedLines;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="outputWriter"></param>
        /// <param name="salarySlipService"></param>
        public SalaryDataFileProcessor( IOutputWriter outputWriter, ISalarySlipService salarySlipService)
        { 
            _outputWriter = outputWriter;
            _salarySlipService = salarySlipService;
            _failedProcessedLines = new List<string>();
        }

        /// <summary>
        /// Passing path of file to process and check if empty
        /// </summary>
        /// <param name="args">Application input</param>
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
        /// Read the input data file and process into a set of monthly payslips 
        /// </summary>
        /// <param name="path">Path to input data file</param>
        public void ProcessFile(string path)
        {
            var lines = File.ReadLines(path).ToArray();
 
            for (int i =0; i < lines.Length ;i++)
            {
                var line = lines[i];

                // Basically printout  the header and also ensure the excpe
                if (lines.Length > 0 && i == 0)
                    PrintHeader();
                try
                {
                    ProcessLine(line);
                }
                catch (Exception ex)
                {
                    // Move this error container or logger
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(" "+ex.Message +" ::: line-number "+ i );
                    sb.AppendLine( " ==> " + line);
                    _failedProcessedLines.Add(sb.ToString());
                }

            }

            if (_failedProcessedLines.Count > 0)
            {
                PrintHeaderFailure();
                _failedProcessedLines.ForEach(
                    failedline =>
                    {
                        _outputWriter.WriteLine(failedline);
                    });
            }
        }

        /// <summary>
        /// Parses the tax line for CSV
        /// </summary>
        /// <param name="line"></param>
        public void ProcessLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] processLine = line.Split(',');

                if (processLine.Length > 0)
                {
                    ProcessingPayLineVm pvm = new ProcessingPayLineVm
                    {
                        FirstName = processLine[Convert.ToInt32(CsvProcessingIndex.FirstName)],
                        LastName = processLine[Convert.ToInt32(CsvProcessingIndex.LastName)],
                        AnnualGrossSalary = processLine[Convert.ToInt32(CsvProcessingIndex.GrossIncome)],
                        SuperAnnuationRate = processLine[Convert.ToInt32(CsvProcessingIndex.SuperAnnuationRate)],
                        StartDateTime = processLine[Convert.ToInt32(CsvProcessingIndex.DatePeriod)],
                    };

                    Customer customer = Mapper.Map<Customer>(pvm);

                    // This is ensure the model mapping from the automapper is valid domain model
                    CustomerValidator cv = new CustomerValidator();
                    var results = cv.Validate(customer);

                    if (results.IsValid)
                    {
                        var outputLine = _salarySlipService.GenerateSalarySlip(customer, TimeFrequency.Monthly);

                        if (outputLine != null)
                        {
                            _outputWriter.WriteLine(" "+outputLine.ToConsoleLineString());
                        }
                        else
                        {
                            throw new Exception("Customer salary slip details not valid");
                        }
                    }
                    else
                    {
                        throw new Exception("Customer details not valid");
                    }

                }
            }
        }

        #region PrintHelpers
            /// <summary>
            /// Print header for console output
            /// </summary>
            private void PrintHeader()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                sb.AppendLine(" Output(name,pay period,gross income,income tax,net income,super): ");
                sb.AppendLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                _outputWriter.WriteLine(sb.ToString());

            }

            /// <summary>
            /// Print Failure header for console output
            /// </summary>
            private void PrintHeaderFailure()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                sb.AppendLine(" Failed Processed Payslips ");
                sb.AppendLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                _outputWriter.WriteLine(sb.ToString());
            }

        #endregion
    }
}
