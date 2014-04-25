// -----------------------------------------------------------------------
// <copyright file="IntExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class IntExtensions
    {
        public static void Times(this int count, Action<int> action)
        {
            for (var i = 0; i < count; i++)
            {
                action(i);
            }
        }

        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, 0, seconds);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return new TimeSpan(0, 0, minutes, 0);
        }

        public static TimeSpan Hours(this int hours)
        {
            return new TimeSpan(0, hours, 0, 0);
        }

        public static TimeSpan Days(this int days)
        {
            return new TimeSpan(days, 0, 0, 0);
        }

        public static TimeSpan Years(this int years)
        {
            return new TimeSpan(years * 365, 0, 0, 0);
        }

    }
}
