using System;

namespace PayApp.Core.Extensions
{
    public static class DecimalExt
    {
        /// <summary>
        /// Round the decimal to nearest hundred
        /// </summary>
        /// <param name="value"></param>
        /// <returns>int</returns>
        public static int RoundValueToNearest100(this decimal value)
        {
            int result = (int)Math.Round(value / 100);
            if (value > 0 && result == 0)
            {
                result = 1;
            }
            return (int)result * 100;
        }

        /// <summary>
        /// Rounds to nearest whole number
        /// </summary>
        /// <param name="value"></param>
        /// <returns>decimal</returns>
        public static decimal RoundToNearestWhole(this decimal value)
        {
            return Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Extension function to convert max as string into the max decimal value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>decimal</returns>
        public static decimal StringToDecimal(this string value)
        {
            return value.Equals("max") ? decimal.MaxValue:System.Convert.ToDecimal(value);
        }
    }
}
