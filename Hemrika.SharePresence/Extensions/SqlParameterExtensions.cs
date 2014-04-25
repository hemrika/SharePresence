// -----------------------------------------------------------------------
// <copyright file="SqlParameterExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.SqlClient;
    using System.Data;

    public static class SqlParameterExtensions
    {
        public static SqlParameter ToParam(this string value, string parameterName, SqlDbType dbType = SqlDbType.NVarChar)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value

            };

            if (!string.IsNullOrEmpty(value))
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this int value, string parameterName, SqlDbType dbType = SqlDbType.Int)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (int.MinValue != value)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this int? value, string parameterName, SqlDbType dbType = SqlDbType.Int)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (value.HasValue == true)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this DateTime value, string parameterName,
                                           SqlDbType dbType = SqlDbType.DateTime)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (DateTime.MinValue != value)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this DateTime? value, string parameterName,
                                           SqlDbType dbType = SqlDbType.DateTime)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (value.HasValue == true)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this double value, string parameterName, SqlDbType dbType = SqlDbType.Float)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (Math.Abs(double.MinValue - value) > double.Epsilon)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this double? value, string parameterName, SqlDbType dbType = SqlDbType.Float)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (value.HasValue == true)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this decimal value, string parameterName,
                                           SqlDbType dbType = SqlDbType.Decimal)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (decimal.MinValue != value)
            {
                param.Value = value;
            }

            return param;
        }

        public static SqlParameter ToParam(this decimal? value, string parameterName,
                                           SqlDbType dbType = SqlDbType.Decimal)
        {
            var param = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = dbType,
                Value = DBNull.Value
            };

            if (value.HasValue == true)
            {
                param.Value = value;
            }

            return param;
        }
    }
}
