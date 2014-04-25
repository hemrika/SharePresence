// -----------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Text;

    public static class ArrayExtensions
    {
        public static bool IsIn<T>(this T element, IEnumerable<T> ienumerable)
        {
            return ienumerable.Contains(element);
        }
        /// <summary>
        /// [js] Array.prototype.push
        /// </summary>
        /// <example>
        /// var len = ArrayExtensions.Push(ref array, new Foo());
        /// </example>
        /// <typeparam name="T">Type of objects in a given array (implicit)</typeparam>
        /// <param name="array">Target array, pass by reference</param>
        /// <param name="item">Item to push into a given array</param>
        /// <returns>Length of the new array, and the original array will contain the new item</returns>
        public static int Push<T>(ref T[] array, T item)
        {
            var len = array.Length;
            var res = new T[len + 1];


            array.CopyTo(res, 0);
            array = res;
            array[len] = item;


            return array.Length;
        }


        /// <summary>
        /// [js] Array.prototype.pop
        /// </summary>
        /// <example>
        /// var popped = ArrayExtensions.Pop(ref array);
        /// </example>
        /// <typeparam name="T">Type of objects in a given array (implicit)</typeparam>
        /// <param name="array">Source array, pass by reference</param>
        /// <returns>Last item in the given array, and the original array will have one less item (LIFO)</returns>
        public static T Pop<T>(ref T[] array)
        {
            var len = array.Length;
            var res = new T[len - 1];
            var popped = array[len - 1];


            if (len > 1)
                for (var i = 0; i < len - 1; i++)
                    res[i] = array[i];


            array = res;


            return popped;
        }


        /// <summary>
        /// [js] Array.prototype.shift
        /// </summary>
        /// <example>
        /// var arg = ArrayExtensions.Shift(ref array);
        /// </example>
        /// <typeparam name="T">Type of objects in a given array (implicit)</typeparam>
        /// <param name="array">Source array, pass by reference</param>
        /// <returns>Last item in the given array, and the original array will have one less item (FIFO)</returns>
        public static T Shift<T>(ref T[] array)
        {
            var len = array.Length;
            var res = new T[len - 1];
            var shifted = array[0];


            while (len > 1)
                res[--len - 1] = array[len];


            array = res;


            return shifted;
        }


        /// <summary>
        /// TODO: [js] Array.prototype.splice
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T[] Splice<T>(ref T[] array, int index, int length, params T[] args)
        {
            throw new NotImplementedException();
        }

        /*
        /// <summary>
        /// [js] Array.prototype.map
        /// </summary>
        /// <example>
        /// var array = new[] { 1, 2, 3, 4, 5 };
        /// array.Map(x => x * 2);
        /// </example>
        /// <typeparam name="T">Type of objects in a given array (implicit)</typeparam>
        /// <param name="array">Source array, pass by reference</param>
        /// <param name="fun">Callback function which will be called for each item in the source array</param>
        /// <returns>A new dynamic[] array which contains the results from the callback function</returns>
        public static dynamic[] Map<T>(this T[] array, Func<T, dynamic> fun)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }


            var len = array.Length;
            var res = new dynamic[len];


            for (var i = 0; i < len; i++)
            {
                var item = array[i];
                res[i] = fun.Invoke(item);
            }


            return res;
        }
        */

        /// <summary>
        /// [js] Array.prototype.filter
        /// </summary>
        /// <example>
        /// array.Filter(x => x > REQUIRED_AGE);
        /// </example>
        /// <typeparam name="T">Type of objects in a given array (implicit)</typeparam>
        /// <param name="array">Source array, pass by reference</param>
        /// <param name="fun">Condition function which will be called for each item in the source array</param>
        /// <returns>A new T[] array which contains the filtered items dictated by the given condition</returns>
        public static T[] Filter<T>(this T[] array, Func<T, bool> fun)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }


            var len = array.Length;
            var res = new T[] { };


            for (var i = 0; i < len; i++)
            {
                var value = array[i];


                if (fun.Invoke(value))
                    Push(ref res, value);
            }


            return res;
        }


        /// <summary>
        /// [js] Array.prototype.join
        /// </summary>
        /// <example>
        /// array.Join("&");
        /// </example>
        /// <param name="array">Source array, pass by reference</param>
        /// <param name="separator">Separator to use when joining each item in the given array</param>
        /// <returns>A string containing all of the items in the given array, separated by the given separator</returns>
        public static string Join(this string[] array, string separator)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (separator == null) throw new ArgumentNullException("separator");


            return String.Join(separator, array);
        }
    }
}
