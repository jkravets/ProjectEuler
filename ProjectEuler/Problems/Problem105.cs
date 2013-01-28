using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem105
    {
        public Dictionary<int, List<List<int>>> ConstraintMaps { get; set; }
        public Problem103 P103 { get; set; }

        public Problem105()
        {
            ConstraintMaps = new Dictionary<int, List<List<int>>>();
            P103 = new Problem103();   
        }

        public void Solve()
        {
            var setsString = File.ReadAllText(@"Assets\\sets.txt");
            var sets = setsString.Split('\n').ToList().Select(s => s.Split(',')).ToList()
                            .Select(s => s.Select(e => Int32.Parse(e)).ToList()).ToList();

            var answer = 0;
            foreach (var set in sets)
            {
                if (IsSpecialSumSet(set))
                {
                    answer += set.Sum();
                }
            }

            var rofl = answer;




        
        }

        public int NegativeCount(List<int> set)
        {
            return set.Count(i => i == -1);
        }
        public int PositiveCount(List<int> set)
        {
            return set.Count(i => i == 1);
        }

        public bool IsSpecialSumSet(List<int> set)
        {
            set = set.OrderBy(i => i).ToList();
            var n = set.Count;
            if (!ConstraintMaps.ContainsKey(n)) 
            {
                ConstraintMaps[set.Count()] = P103.ConstraintEquations(n);
            }
            var constraints = ConstraintMaps[n];

            foreach (var constraint in constraints)
            {
                var value = 0;
                for (int i = 0; i < n; i++)
                {

                    value += set[i] * constraint[i];
                }

                var nCount = NegativeCount(constraint);
                var pCount = PositiveCount(constraint);

                if (nCount == pCount && value == 0)
                {
                    return false;
                }

                if (pCount > nCount && value <= 0)
                {
                    return false;
                }
            }

            return true;

        }
    }
}
