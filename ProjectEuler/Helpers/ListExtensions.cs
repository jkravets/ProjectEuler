using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public static class ListExtensions
    {
        public static void RemoveRange<T>(this IList<T> list, IList<T> other)
        {
            for (int i = 0; i < other.Count; i++)
            {
                list.Remove(other[i]);
            }
        }

        public static bool ContainsRange<T>(this IList<T> list, IList<T> other)
        {
            var contains = true;
            foreach (var o in other)
            {
                contains = contains && list.Contains(o);
                if (!contains) break;
            
            }
            return contains;
        }

        public static bool ContainsAnyRange<T>(this IList<T> list, IList<T> other)
        {
            var contains = false;
            foreach (var o in other)
            {
                contains = contains || list.Contains(o);
                if (contains) break;
            }
            return contains;
        }

        
        public static List<List<T>> Combinations<T>(this IList<T> list, int size)
        {
            var combinations = new List<List<T>>();
            if (size == 0)
            {
                return combinations;
            }

            foreach (var i in list)
            {
                if (size > 1)
                {
                    var subList = list.ToList();
                    subList.Remove(i);
                    foreach (var subCombination in Combinations(subList, size - 1))
                    {
                        subCombination.Add(i);
                        combinations.Add(subCombination);
                    }
                }
                else 
                {
                    combinations.Add(new List<T>() { i });
                }
            }

            return combinations;
        }
    }
}
