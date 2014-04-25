// -----------------------------------------------------------------------
// <copyright file="RangeExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static partial class RangeExtensions
    {

        public static bool InRange(this int target, int from, int to, int uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this long target, long from, long to, long uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this double target, double from, double to, double uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this float target, float from, float to, float uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this decimal target, decimal from, decimal to, decimal uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }

        public static bool InRange(this uint target, uint from, uint to, uint uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }

        public static bool InRange(this ushort target, ushort from, ushort to, ushort uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this ulong target, ulong from, ulong to, ulong uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this short target, short from, short to, short uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this byte target, byte from, byte to, byte uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this sbyte target, sbyte from, sbyte to, sbyte uncertainty)
        {
            return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
        }
        public static bool InRange(this int target, int from, int to)
        {
            return InRange(target, from, to, 0);
        }
        public static bool InRange(this decimal target, decimal from, decimal to)
        {
            return InRange(target, from, to, 0.0M);
        }
        public static bool InRange(this double target, double from, double to)
        {
            return InRange(target, from, to, 0.0);
        }
        public static bool InRange(this float target, float from, float to)
        {
            return InRange(target, from, to, 0.0f);
        }
        public static bool InRange(this long target, long from, long to)
        {
            return InRange(target, from, to, 0L);
        }
        public static bool InRange(this short target, short from, short to)
        {
            return InRange(target, from, to, (short)0);
        }
        public static bool InRange(this uint target, uint from, uint to)
        {
            return InRange(target, from, to, (uint)0);
        }
        public static bool InRange(this ushort target, ushort from, ushort to)
        {
            return InRange(target, from, to, (ushort)0);
        }
        public static bool InRange(this ulong target, ulong from, ulong to)
        {
            return InRange(target, from, to, (ulong)0);
        }
        public static bool InRange(this byte target, byte from, byte to)
        {
            return InRange(target, from, to, (byte)0);
        }
        public static bool InRange(this sbyte target, sbyte from, sbyte to)
        {
            return InRange(target, from, to, (sbyte)0);
        }
    }
}
