// -----------------------------------------------------------------------
// <copyright file="XElementExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Globalization;

    public static class XElementExtensions
    {
        public static string ToString(this XElement element, string emptyValue)
        {
            string returnValue = string.Empty;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = element.Value;
            }

            return returnValue;
        }

        public static decimal ToDecimal(this XElement element, decimal emptyValue)
        {
            decimal returnValue;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = decimal.Parse(element.Value);
            }
            else
            {
                returnValue = emptyValue;
            }

            return returnValue;
        }

        public static decimal? ToNullableDecimal(this XElement element)
        {
            decimal? returnValue = null;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = decimal.Parse(element.Value);
            }

            return returnValue;
        }

        public static int ToInt(this XElement element, int emptyValue)
        {
            int returnValue;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = int.Parse(element.Value);
            }
            else
            {
                returnValue = emptyValue;
            }

            return returnValue;
        }

        public static int? ToNullableInt(this XElement element)
        {
            int? returnValue = null;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = int.Parse(element.Value);
            }

            return returnValue;
        }

        public static DateTime ToDateTime(this XElement element, DateTime emptyValue)
        {
            DateTime returnValue;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = DateTime.Parse(element.Value, null, DateTimeStyles.RoundtripKind);
            }
            else
            {
                returnValue = DateTime.MinValue;
            }

            return returnValue;
        }

        public static DateTime? ToNullableDateTime(this XElement element)
        {
            DateTime? returnValue = null;

            if (string.IsNullOrEmpty(element.Value))
            {
                returnValue = DateTime.Parse(element.Value, null, DateTimeStyles.RoundtripKind);
            }

            return returnValue;
        }


    }
}
