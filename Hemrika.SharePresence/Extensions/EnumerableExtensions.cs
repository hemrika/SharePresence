﻿// -----------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> iterator, Func<T, TResult> mapper)
        {
            return new List<T>(iterator).Select(mapper).ToArray();
        }


        public static IEnumerable<TResult> Map<TResult>(this IEnumerable iterator, Func<object, TResult> mapper)
        {
            var genericList = new List<object>();
            foreach (var item in iterator)
            {
                genericList.Add(item);
            }
            return Map(genericList, mapper);
        }

        /// <summary>
        /// Split the collection into collections of size chunkSize.
        /// </summary>
        /// <remarks>Uses yield return/break.</remarks>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source,
            int chunkSize)
        {
            source.NullArgumentCheck("source");
            chunkSize.ArgumentRangeCheck("chunkSize");
            for (int start = 0; start < source.Count(); start += chunkSize)
            {
                // note: skip/take is slow. not O(n) but (n/chunkSize)^2.
                // yield return source.Skip(chunk).Take(chunkSize);
                yield return source.Chunk(chunkSize, start);
            }
        }
        /// <summary>
        /// Yield the next chunkSize elements starting at start and break if no more elements left.
        /// </summary>
        private static IEnumerable<T> Chunk<T>(this IEnumerable<T> source,
            int chunkSize, int start)
        {
            source.NullArgumentCheck("source");
            chunkSize.ArgumentRangeCheck("chunkSize");
            for (int i = 0; i < chunkSize; i++)
            {
                if (start + i == source.Count())
                {
                    yield break;
                }
                yield return source.ElementAt(start + i);
            }
        }
        /// <summary>
        /// Returns true if collection is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
        /// <summary>
        /// Split a collection into n parts.
        /// </summary>
        /// <remarks>http://stackoverflow.com/questions/438188/split-a-collection-into-n-parts-with-linq</remarks>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int parts)
        {
            source.NullArgumentCheck("source");
            parts.ArgumentRangeCheck("parts");
            // requires more space for objects
            //var splits_index = source.Select((x, index) => new { x, index = index })
            //    .GroupBy(x => x.index % parts)
            //    .Select(g => g.Select(x => x));
            int i = 0;
            var splits = source.GroupBy(x => i++ % parts)
                .Select(g => g.Select(x => x));
            return splits;
        }
        /// <summary>
        /// Returns true if list is ascendingly or descendingly sorted.
        /// </summary>
        public static bool IsSorted<T>(this IEnumerable<T> source)
            where T : IComparable
        {
            source.NullArgumentCheck("source");
            // make an array to avoid inefficiency due to ElementAt(index)
            var sourceArray = source.ToArray();
            if (sourceArray.Length <= 1)
            {
                return true;
            }
            var isSorted = false;
            // asc test
            int i;
            for (i = 1; i < sourceArray.Length; i++)
            {
                if (sourceArray[i - 1].CompareTo(sourceArray[i]) > 0)
                {
                    break;
                }
            }
            isSorted = sourceArray.Length == i;
            if (isSorted)
            {
                return true;
            }
            // desc test
            for (i = 1; i < sourceArray.Length; i++)
            {
                if (sourceArray[i - 1].CompareTo(sourceArray[i]) < 0)
                {
                    break;
                }
            }
            isSorted = sourceArray.Length == i;
            return isSorted;
        }
        /// <summary>
        /// Returns the only two elements of a sequence.
        /// </summary>
        public static IEnumerable<T> Double<T>(this IEnumerable<T> source)
        {
            source.NullArgumentCheck("source");
            return XOrDefaultInternal(source, count: 2, emptyCheck: false);
        }
        /// <summary>
        ///  Returns the only two elements of a sequence that satisfy a specified condition.
        /// </summary>
        public static IEnumerable<T> Double<T>(this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            source.NullArgumentCheck("source");
            predicate.NullArgumentCheck("predicate");
            return Double(source.Where(predicate));
        }
        /// <summary>
        /// Returns the only two elements of a sequence, or a default value if the sequence is empty.
        /// </summary>
        public static IEnumerable<T> DoubleOrDefault<T>(this IEnumerable<T> source)
        {
            source.NullArgumentCheck("source");
            return XOrDefaultInternal(source, count: 2, emptyCheck: true);
        }
        /// <summary>
        /// Returns the only two elements of a sequence that satisfy a specified condition 
        /// or a default value if no such elements exists.
        /// </summary>
        public static IEnumerable<T> DoubleOrDefault<T>(this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            source.NullArgumentCheck("source");
            predicate.NullArgumentCheck("predicate");
            return DoubleOrDefault(source.Where(predicate));
        }
        /// <summary>
        /// Returns the only <paramref name="count"/> elements of a sequence 
        /// or a default value if no such elements exists depending on <paramref name="emptyCheck"/>.
        /// </summary>
        private static IEnumerable<T> XOrDefaultInternal<T>(IEnumerable<T> source,
            int count, bool emptyCheck)
        {
            source.NullArgumentCheck("source");
            count.ArgumentRangeCheck("count");
            if (emptyCheck)
            {
                if (source.Count() == 0)
                {
                    return null;
                }
            }
            if (source.Count() == count)
            {
                return source;
            }
            throw new InvalidOperationException(
                string.Format("The input sequence does not contain {0} elements.", count)
                );
        }
        /// <summary>
        /// Returns a new collection with items shuffled in O(n) time.
        /// </summary>
        /// <remarks>
        /// Fisher-Yates shuffle
        /// http://stackoverflow.com/questions/1287567/is-using-random-and-orderby-a-good-shuffle-algorithm
        /// </remarks>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            source.NullArgumentCheck("source");
            rng.NullArgumentCheck("rng");
            var items = source.ToArray();
            for (int i = items.Length - 1; i >= 0; i--)
            {
                var swapIndex = rng.Next(i + 1);
                yield return items[swapIndex];
                // no need to swap fully as items[swapIndex] is not used later
                items[swapIndex] = items[i];
            }
        }
        /// <summary>
        /// Returns a new collection with items shuffled in O(n) time.
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return Shuffle(source, new Random());
        }

    }
}
