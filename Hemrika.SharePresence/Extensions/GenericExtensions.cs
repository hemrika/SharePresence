/*
// -----------------------------------------------------------------------
// <copyright file="GenericExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static partial class GenericExtensions
    {
        public static bool EqualsStructure<T>(this T a, T b)
            where T : IStructuralEquatable
        {
            return a.Equals(b, StructuralComparisons.StructuralEqualityComparer);
        }


        public static int CompareStructureTo<T>(this T a, T b)
            where T : IStructuralComparable
        {
            return a.CompareTo(b, StructuralComparisons.StructuralComparer);
        }

        public static IEnumerable<T> Append<T>(this T value, IEnumerable<T> elements)
        {
            yield return value;
            foreach (var v in elements)
                yield return v;
        }
        public static IEnumerable<T> Append<T>(this T value, params T[] elements)
        {
            return Append(value, (IEnumerable<T>)elements);
        }
        //allows code to executed inline of a statement without disrupting the statement
        public static T Then<T>(this T value, Action code)
        {
            code();
            return value;
        }
        public static T Then<T>(this T value, Action<T> code)
        {
            code(value);
            return value;
        }
        //Performs an operation on a value inline, this is useful for values that can't
        //be stored into a separate variable before hand
        public static T Then<T>(this T value, Func<T, T> modCode)
        {
            return modCode(value);
        }
        //this is an inline modification chain...be forewarned
        public static T Then<T>(this T value, params Action<T>[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
                actions[i](value);
            return value;
        }
        public static T Then<T>(this T value, params Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
                actions[i]();
            return value;
        }
        public static T Then<T>(this T value, params Func<T, T>[] functions)
        {
            T output = value;
            for (int i = 0; i < functions.Length; i++)
                output = functions[i](output);
            return output;
        }

    }
}
*/