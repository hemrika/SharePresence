// -----------------------------------------------------------------------
// <copyright file="DateExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class DateExtensions
    {
        /*
        public static int LastDayInMonth(this DateTime date)
        {
            string rootMonthString = string.Format("{0}/1/{1}", date.Month, date.Year);
            return rootMonthString.ToDate().AddMonths(1).AddDays(-1).Day;
        }

        public static bool IsLastDayInMonth(this DateTime date)
        {
            return date.Day == date.LastDayInMonth();


        }
        */
        /*
        public static DateTime NextMonth(this DateTime startDate)
        {
            return GetResultingDate(startDate, 1);
        }


        public static DateTime LastMonth(this DateTime startDate)
        {
            return GetResultingDate(startDate, -1);
        }
        */
        /*
        public static DateTime GetAniversaryDate(this DateTime sourceDate)
        {
            return GetAniversaryDate(sourceDate, DateTime.Now);
        }

        public static DateTime GetAniversaryDate(this DateTime sourceDate, DateTime destination)
        {
            int day = destination.LastDayInMonth() < sourceDate.Day ? destination.LastDayInMonth() : sourceDate.Day;
            return string.Format("{0}/{1}/{2}", destination.Month, day, destination.Year).ToDate();
        }
        */

        /*
        private static DateTime GetResultingDate(DateTime startDate, int monthLocation)
        {
            DateTime nextMonth = startDate.AddMonths(monthLocation);
            DateTime resultingDate;


            if (startDate.Day > nextMonth.LastDayInMonth())
                resultingDate = string.Format("{0}/{1}/{2}", nextMonth.Month, nextMonth.LastDayInMonth(), nextMonth.Year).ToDate();
            else
                resultingDate = string.Format("{0}/{1}/{2}", nextMonth.Month, startDate.Day, nextMonth.Year).ToDate();
            return resultingDate;
        }
        */
    }
}
