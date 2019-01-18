using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuoteParser
{
    internal static class CollectionExtensions
    {
        public static IList<T> SubList<T>(this IList<T> list, int fromIndex, int toIndex)
        {
            return list.Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public static int LastIndex<T>(this IList<T> list)
        {
            return list.Count - 1;
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }

        public static string JoinToString<T>(this IEnumerable<T> list,
            string separator = ", ",
            string prefix = "",
            string postfix = "",
            Func<T, string> transform = null)
        {
            var sb = new StringBuilder(prefix);

            sb.Append(transform != null ? string.Join(separator, list.Select(transform)) : string.Join(separator, list));

            sb.Append(postfix);

            return sb.ToString();
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> callback)
        {
            foreach (var item in items)
            {
                callback(item);
            }
        }

        public static void ForEachIndexed<T>(this IEnumerable<T> items, Action<int, T> callback)
        {
            int i = 0;

            foreach (var item in items)
            {
                callback(i++, item);
            }
        }

        public static IList<T> Sorted<T>(this ICollection<T> items)
        {
            return items.OrderBy(i => i).ToList();
        }

        public static int IndexOfFirst<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            int index = 0;

            foreach (var item in items)
            {
                if (predicate(item))
                    return index;

                index++;
            }

            return -1;
        }

        public static int IndexOfLast<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            return items.Reverse().IndexOfFirst(predicate);
        }
    }
}
