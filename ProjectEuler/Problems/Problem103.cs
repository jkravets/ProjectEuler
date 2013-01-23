using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
    public class Problem103
    {

        //Trying to solve the problem using LPSolve and .lp file
        public void Solve()
        {

            var variables = 3;
            var constraints = ConstraintEquations(variables);
            
            var totalVariables = constraints.Count() + 1 + variables;
            var A = new float[1 + constraints.Count(),totalVariables];
            var b = new float[totalVariables];

            //Create the first constraint line
            float e = 0.00001F;
            for (int i = 1; i < b.Count(); i++)
            {
                b[i] = e;
            }
            A[0, 0] = 1;
            for (int j = 1; j <= variables; j++)
            {
                A[0, j] = -1;
            }

            var si = 1;
            foreach (var constraint in constraints)
            {

                for (int j = 1; j <= variables; j++)
                {
                    A[si, j] = constraint[j - 1];
                }
                A[si, si + variables] = 1;
                si++;
            }

            var matrixA = new DenseMatrix(A);
            var vectorb = new DenseVector(b);

            var coefficents = matrixA.QR().Solve(vectorb);



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
