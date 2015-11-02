using System;
using System.IO;
using System.Linq;

namespace PayApp.Test.Helpers
{
    public class HelperMethods
    {
        /// <summary>
        /// Helps with the file being read from the particular location for test cases
        /// </summary>
        /// <param name="testDataFolder"></param>
        /// <returns>string</returns>
        public static string GetTestDataFolder(string testDataFolder)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 3));
            return Path.Combine(projectPath, testDataFolder);
        }
    }
}
