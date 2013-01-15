using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem088
    {
        /*
         * 
         * k <= minps(k) <= 2k
         * 
         * 
         * 
         * 
         */
        public void Answer()
        {           
            var k = 12000;


            var sets = new int[2 * k];
            for (int i = 0; i < 2 * k; i++)
            {
                sets[i] = int.MaxValue;
            }

            //All possible numbers
            for (int number = 2; number <= 2 * k; number++)
            {
                var factorizations = IntegerFactorizations(number);
                foreach (var factorization in factorizations)
                {
                    var ones = number - factorization.Sum();
                    var index = factorization.Count + ones;
                    sets[index] = number < sets[index] ? number : sets[index];                   
                }
            }

            var answer = sets.Skip(2).Take(k - 2).Distinct().Sum();

                        
        }

        public List<List<int>> IntegerFactorizations(int number)
        {
            var factorizations = new List<List<int>>();
            factorizations.Add(new List<int>() { number });

            for (int i = 2; i <= number/2.0; i++)
            {
                var result = (double)number/(double)i;

                if ((Math.Floor(result) - result) == 0)
                {
                    foreach (var fact in IntegerFactorizations((int)result))
                    {
                        fact.Insert(0, i);
                        fact.Sort();
                        factorizations.Add(fact);
                    }                    
                }
            }
           
            return factorizations.Distinct(new FactorizationComparer()).ToList();
        }

        public class FactorizationComparer : IEqualityComparer<List<int>>
        {

            public bool Equals(List<int> x, List<int> y)
            {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] != y[i]) return false;
                }
                return true;
            }

            public int GetHashCode(List<int> obj)
            {
                return obj.Aggregate(0, (hash, i) => hash ^ i.GetHashCode());
            }
        }



    }

}
