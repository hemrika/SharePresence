﻿// -----------------------------------------------------------------------
// <copyright file="IEnumerableTExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class IEnumerableTExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            return new HashSet<T>(collection, comparer);
        }

        public static HashSet<string> ToHashSetInvariant(this IEnumerable<string> collection)
        {
            return new HashSet<string>(collection, StringComparer.OrdinalIgnoreCase);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> value)
        {
            if (value == null)
                return new T[] { };
            else
                return value;
        }

        [Obsolete("Use ToDictionary instead")]
        public static IDictionary<TKey, TValue> ToStandardDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
        {
            return values.ToDictionary(p => p.Key, p => p.Value, comparer);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
        {
            return values.ToDictionary(p => p.Key, p => p.Value, comparer);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            return IEnumerableTExtensions.ToDictionary(values, EqualityComparer<TKey>.Default);
        }

        public static IDictionary<string, TValue> ToInvariantDictionary<TValue>(this IEnumerable<KeyValuePair<string, TValue>> values)
        {
            return values.ToDictionary(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares two unordered lists of key value pairs add returns the differences by comparing items with the same keys. Typically you'd pass in dictionaries.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue[]>> Diff<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> a, IEnumerable<KeyValuePair<TKey, TValue>> b, DiffOptions options)
        {
            // Full outer join
            var leftAndBoth =
                from pa in a
                join pb in b on pa.Key equals pb.Key into c
                from pc in c.DefaultIfEmpty()
                select new { Key = pa.Key, A = pa.Value, B = pc.Value };

            var rightOnly =
                from pb in b
                join pa in a on pb.Key equals pa.Key into c
                from pc in c.DefaultIfEmpty()
                where default(KeyValuePair<TKey, TValue>).Equals(pc)
                select new { Key = pb.Key, A = pc.Value, B = pb.Value };

            bool includeLeftOnly = IsSet(options, DiffOptions.LeftOnly);
            bool includeRightOnly = IsSet(options, DiffOptions.RightOnly);
            bool includeModified = IsSet(options, DiffOptions.Modified);
            bool includeMatching = IsSet(options, DiffOptions.Matching);

            var all = leftAndBoth.Concat(rightOnly);

            var items = from item in all
                        where
                        (
                            (includeLeftOnly && item.A != null && item.B == null)
                            || (includeRightOnly && item.A == null && item.B != null)
                            || (includeModified && item.A != null && item.B != null && !item.A.Equals(item.B))
                            || (includeMatching && ((item.A == null && item.B == null) || (item.A != null && item.A.Equals(item.B))))
                        )
                        select item;

            return items.Select(p => new KeyValuePair<TKey, TValue[]>(p.Key, new[] { p.A, p.B }));
        }

        static bool IsSet(DiffOptions value, DiffOptions toCheck)
        {
            return (value & toCheck) == toCheck;
        }
    }

}
