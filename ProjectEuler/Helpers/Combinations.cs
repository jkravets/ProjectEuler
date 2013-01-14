using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    public static class CombinationExtension
    {
        public static List<List<T>> Combinations<T>(this List<T> list, int size, IEqualityComparer<List<T>> comparer)
        {
            var combinations = new List<List<T>>();
            foreach (var item in list)
            {
                var listCopy = list.ToList();
                listCopy.Remove(item);
                if (size > 1)
                {
                    foreach (var childCombo in Combinations(listCopy, size - 1, comparer))
                    {
                        var combination = new List<T>() { item };
                        combination.AddRange(childCombo);
                        combinations.Add(combination);
                    }
                }
                else
                {
                    combinations.Add(new List<T>() { item });
                }
            }

            foreach (var combination in combinations)
            {
                combination.Sort();
            }

            return combinations.Distinct(comparer).ToList();


        }


        
    }

    public class IntegerListCompare : IEqualityComparer<List<int>>
    {

        public bool Equals(List<int> x, List<int> y)
        {
            if (x.Count != y.Count) return false;

            for (var i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i]) return false;
            }

            return true;
        }

        public int GetHashCode(List<int> obj)
        {
            return obj.Aggregate(0, (hash, item) => hash + item.GetHashCode());
        }
    }
}
