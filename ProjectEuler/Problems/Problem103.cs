using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem103
    {
        public void Solve()
        {

            var constraints = ConstraintEquations(3);

            //This is the maximum sum we are looking for
            var max = SetGuess(7).Sum();
        
        
        }



        public List<List<int>> ConstraintEquations(int n)
        {            
            //Get all combinations of n
            var constraints = ConstraintCombination(n);


            //Apply the constraint rule checks
            Func<List<int>, bool> CheckZeros = (c) =>
            {
                return !c.All(a => a == 0);
            };

            Func<List<int>, bool> CheckPositives = (c) =>
            {
                return !c.All(a => a >= 0);
            };

            Func<List<int>, bool> CheckZeroCount = (c) =>
            {
                return c.Count(a => a > 0) >= c.Count(a => a == -1);
            };

            Func<List<int>, bool> GroupCheck = (c) =>
            {
                var positives = c.Count(a => a > 0);
                var negatives = c.Count(a => a == -1);
                if (positives == negatives && positives == 1)
                {
                    var indexP = c.IndexOf(1);
                    var indexN = c.IndexOf(-1);
                    return indexP == indexN + 1;
                }
                return true;
            };


            return constraints.Where(CheckZeros).Where(CheckPositives).Where(CheckZeroCount).Where(GroupCheck).ToList();

        }

        private List<List<int>> ConstraintCombination(int n)
        {
            var constraints = new List<List<int>>();
            if (n >= 1)
            {
                for (int i = -1; i <= 1; i++)
                {
                    if (n > 1)
                    {
                        foreach (var subConstraint in ConstraintCombination(n - 1))
                        {
                            var constraint = new List<int>() { i };
                            constraint.AddRange(subConstraint);
                            constraints.Add(constraint);
                        }
                    }
                    else 
                    {
                        constraints.Add(new List<int>() { i });
                    }
                }
            }
            return constraints;
        }


        public List<int> SetGuess(int n)
        {

            var set = new List<int>() { 1 };
            if (n == 1)
            {
                return set;
            }
            set.Add(2);
            if (n == 2)
            {
                return set;
            }


            for(int i = 2; i < n; i++)
            {
            
                //Find the middle value in the set
                var mid = (int)Math.Ceiling((set.Count - 1) / 2.0);
                var b = set[mid];

                set.Insert(0, 0);
                set = set.Select(a => a + b).ToList();
                
            }


            return set;

        }
    }
}
