// -----------------------------------------------------------------------
// <copyright file="DataRowExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    public static class DataRowExtensions
    {
        public static string ToString(this DataRow row, string columnName)
        {
            string returnValue = string.Empty;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToString(row[columnName]);
            }

            return returnValue;
        }

        public static int ToInt(this DataRow row, string columnName)
        {
            int returnValue = Int32.MinValue;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToInt32(row[columnName]);
            }

            return returnValue;
        }

        public static int? ToNullableInt(this DataRow row, string columnName)
        {
            int? returnValue = null;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToInt32(row[columnName]);
            }

            return returnValue;
        }

        public static double ToDouble(this DataRow row, string columnName)
        {
            double returnValue = double.MinValue;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToDouble(row[columnName]);
            }

            return returnValue;
        }

        public static double? ToNullableDouble(this DataRow row, string columnName)
        {
            double? returnValue = null;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToDouble(row[columnName]);
            }

            return returnValue;
        }

        public static decimal ToDecimal(this DataRow row, string columnName)
        {
            decimal returnValue = decimal.MinValue;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToDecimal(row[columnName]);
            }

            return returnValue;
        }

        public static decimal? ToNullableDecimal(this DataRow row, string columnName)
        {
            decimal? returnValue = null;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToDecimal(row[columnName]);
            }

            return returnValue;
        }

        public static DateTime ToDateTime(this DataRow row, string columnName)
        {
            DateTime returnValue = DateTime.MinValue;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToDateTime(row[columnName]);
            }

            return returnValue;
        }

        public static DateTime? ToNullableDateTime(this DataRow row, string columnName)
        {
            DateTime? returnValue = null;

            if (row[columnName] != DBNull.Value)
            {
                returnValue = Convert.ToDateTime(row[columnName]);
            }

            return returnValue;
        }


    }

}
