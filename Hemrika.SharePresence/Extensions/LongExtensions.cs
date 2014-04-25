﻿// -----------------------------------------------------------------------
// <copyright file="LongExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class LongExtensions
    {
        public static string FormatBytes(this long bytes)
        {
            const long scale = 1024;

            var orders = new[] { "EB", "PB", "TB", "GB", "MB", "KB", "bytes" };


            var max = (long)Math.Pow(scale, (orders.Length - 1));

            foreach (var order in orders)
            {
                if (bytes > max)
                {
                    return string.Format("{0:##.##} {1}", Decimal.Divide(bytes, max), order);
                }


                max /= scale;
            }


            return "0 bytes";
        }
    }
}
