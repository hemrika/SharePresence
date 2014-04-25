using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Hemrika.SharePresence.Common.Ribbon
{
    /// <summary>
    /// 
    /// </summary>
    internal static class LinqExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class ListNode<T>
        {
            public T Value { get; private set; }
            public ListNode<T> Next { get; private set; }

            public ListNode(T value, ListNode<T> next)
            {
                Value = value;
                Next = next;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootNodes"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static IEnumerable<T> WithDescendants<T>(this IEnumerable<T> rootNodes, Func<T, IEnumerable<T>> next)
        {
            if (rootNodes == null)
                yield break;

            ListNode<T> list = null;
            foreach (T listNode in rootNodes)
            {
                list = new ListNode<T>(listNode, list);
            }

            while (list != null)
            {
                var currentNode = list;
                var nextNode = list.Next;
                yield return currentNode.Value;

                ListNode<T> tmpList = null;
                IEnumerable<T> nextNodes = next(currentNode.Value);
                if (nextNodes != null)
                {
                    foreach (var v in nextNodes)
                    {
                        tmpList = new ListNode<T>(v, tmpList);
                    }
                }

                if (tmpList != null)
                {
                    for (; tmpList != null; tmpList = tmpList.Next)
                    {
                        nextNode = new ListNode<T>(tmpList.Value, nextNode);
                    }
                }
                list = nextNode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootNodes"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static IEnumerable WithDescendants(this IEnumerable rootNodes, Func<object, IEnumerable> next)
        {
            if (rootNodes == null)
                yield break;

            ListNode<object> list = null;
            foreach (object listNode in rootNodes)
            {
                list = new ListNode<object>(listNode, list);
            }

            while (list != null)
            {
                var currentNode = list;
                var nextNode = list.Next;
                yield return currentNode.Value;

                ListNode<object> tmpList = null;
                IEnumerable nextNodes = next(currentNode.Value);
                if (nextNodes != null)
                {
                    foreach (var v in nextNodes)
                    {
                        tmpList = new ListNode<object>(v, tmpList);
                    }
                }

                if (tmpList != null)
                {
                    for (; tmpList != null; tmpList = tmpList.Next)
                    {
                        nextNode = new ListNode<object>(tmpList.Value, nextNode);
                    }
                }
                list = nextNode;
            }
        }
    }
}
