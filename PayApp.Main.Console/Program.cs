using System.Collections.Generic;
using Microsoft.Practices.Unity;
using PayApp.Core.Models;
using PayApp.Core.Presentation.Extensions;
using PayApp.Data.Rates;
using PayApp.Services.FileProcessor;
using PayApp.Services.OutputWriter;
using PayApp.Services.SalarySlip;
using PayApp.Services.Tax;

namespace PayApp.Main.Console
{
    class Program
    {

        /// <summary>
        /// Method bootstraps application and runs the application
        /// </summary>
        /// <param name="args">Command line arguements passed in</param>
        static void Main(string[] args)
        {
            AutoMapperExtensions.Build();

            var container = new UnityContainer();

            container.RegisterType<IOutputWriter, ConsoleOutputWriter>();

            if (args.Length == 2)
            {

                //Conditional Bootstrapping of the services
                container.RegisterType<IRateDatasSource, TaxRateDb>(new InjectionConstructor(new List<TaxBracket>()));
                container.RegisterType<ISalarySlipService, MonthlySalarySlipService>();
                container.RegisterType<IFileProcessor, SalaryDataFileProcessor>("SalaryProcessor");
                container.RegisterType<IFileProcessor, RatesDataFileProcessor>("RatesProcessor");
                container.RegisterType<ITaxService, TaxCalculationService>();

                //Load tax rates from file
                container.Resolve<IFileProcessor>("RatesProcessor").Run(args[0]);

                //Read and Process salary from file
                container.Resolve<IFileProcessor>("SalaryProcessor").Run(args[1]);

            }
            else
            {
                //printing output writer
                container.Resolve<IOutputWriter>().WriteLine("Application Error - Please provide two paths to a rates and paypacket csv respectively");
            }


            //Console readkey
            System.Console.ReadKey();

            //Dispose Unity Container
            container.Dispose();
        }
    }
}
