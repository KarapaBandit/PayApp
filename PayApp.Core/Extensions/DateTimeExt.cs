using System;
using System.Globalization;

namespace PayApp.Core.Extensions
{
    public static class DateTimeExt
    {

        public const string DateTimeFormat = "dd MMMM yyyy";

        public const string DateTimeFormatExDays = "MMMM yyyy";

        /// <summary>
        /// Convert string to DateTime as expected format. (dd MMMM yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ConvertStringtoDateTime(this string date)
        {

            DateTime parsedDateTime = DateTime.ParseExact(date.Trim(), DateTimeFormat, CultureInfo.InvariantCulture,
                                       DateTimeStyles.None);

            return parsedDateTime;
        }

        /// <summary>
        /// Checks if the dateTime is within the date range
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public static bool WithinDateTimeRange(this DateTime input, DateTime startDateTime, DateTime endDateTime)
        {
            return (startDateTime <= input && endDateTime >= input);
        }
    }
}
