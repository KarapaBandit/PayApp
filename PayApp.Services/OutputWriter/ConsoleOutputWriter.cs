using System;

namespace PayApp.Services.OutputWriter
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        /// <summary>
        /// Checks is string is null, if not write the string to console
        /// </summary>
        /// <param name="output"></param>
        public void WriteLine(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                return;
            }
            Console.WriteLine(output);
        }
   
    }
}
