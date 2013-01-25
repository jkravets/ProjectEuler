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

            var guess = SetGuess(7);
            
            var variables = 7;
            var constraints = ConstraintEquations(variables);            
            var totalVariables = constraints.Count() + 1 + variables;
            var e = 0.1;
            var M = 10000;

            var lpString = "";
            lpString += "min: ";
            for (int i = 1; i <= variables; i++)
            {
                lpString += string.Format("+x{0} ", i);
            }
            lpString += ";";

            lpString += "\n";
            var constraintCount = 1;
            var numberNots = 0;
            foreach (var constraint in constraints)
            {
                lpString += "\n";


                var positives = constraint.Count(i => i == 1);
                var negatives = constraint.Count(i => i == -1);
                if (positives > 1 && positives == negatives)
                {
                    numberNots++;
                    lpString += "\n";
                    //Write this one as an inequality
                    for (int i = 1; i <= variables; i++)
                    {
                        if (constraint[i - 1] != 0)
                        {

                            lpString += string.Format("{0}x{1} ",
                                constraint[i - 1] > 0 ? "+" : "-",
                                i);

                        }
                    }
                    lpString += " + " + M + "*i" + constraintCount;
                    lpString += " >= " + e + ";";
                    lpString += "\n";

                    //Write this one as an inequality
                    for (int i = 1; i <= variables; i++)
                    {
                        if (constraint[i - 1] != 0)
                        {
                            //Swap the signs
                            lpString += string.Format("{0}x{1} ",
                                constraint[i - 1] > 0 ? "-" : "+",
                                i);

                        }
                    }

                    lpString += string.Format(" + {0} - {0}*i{1}",M,constraintCount);
                    lpString += " >= " + e + ";";
                    lpString += "\n";

                }
                else
                {

                    for (int i = 1; i <= variables; i++)
                    {
                        if (constraint[i - 1] != 0)
                        {

                            lpString += string.Format("{0}x{1} ",
                                constraint[i - 1] > 0 ? "+" : "-",
                                i);

                        }
                    }
                    lpString += " >= " + e + ";";
                }
                constraintCount++;
            }

            lpString += "\n";
            lpString += "int ";
            for (int i = 1; i <= variables; i++)
            {
                lpString += string.Format("x{0},", i);
            }

            lpString = lpString.Remove(lpString.Length - 1, 1);
            lpString += ";";

            if (numberNots > 0)
            {

                lpString += "\n";
                lpString += "bin ";
                constraintCount = 1;
                foreach (var constraint in constraints)
                {
                    var positives = constraint.Count(i => i == 1);
                    var negatives = constraint.Count(i => i == -1);
                    if (positives > 1 && positives == negatives)
                    {
                        lpString += string.Format("i{0},", constraintCount);
                    }
                    constraintCount++;

                }

                lpString = lpString.Remove(lpString.Length - 1, 1);
                lpString += ";";
            }

            //Take this string and place it inside the LPSolve program. After some time the solution should be calculated.
            Console.WriteLine(lpString);
        
        
        }



        public List<List<int>> ConstraintEquations(int n)
        {            
            //Get all combinations of n
            var constraints = ConstraintCombination(n);

            
            //We want more positives than negatives
            Func<List<int>, bool> PositivesFlowCheck = (c) =>
            {
                var positives = c.Count(a => a == 1);
                var negatives = c.Count(a => a == -1);
                return positives >= negatives;
            };

            //The right hand side is always larger than left hand side
            //Only check one to the right over
            Func<List<int>, bool> RHSLargeCheck = (c) =>
            {
                var positives = c.Count(a => a == 1);
                var negatives = c.Count(a => a == -1);
                if (positives == 1 && negatives == 1)
                {
                    var indexP = c.IndexOf(1);
                    var indexN = c.IndexOf(-1);
                    return indexP == indexN + 1;
                }
                return true;
            };

            //Apply the constraint rule checks
            Func<List<int>, bool> CheckSingleType = (c) =>
            {
                return !c.All(a => a == 0) && !c.All(a => a >= 0) && !c.All(a => a <= 0);
            };

            Func<List<int>, bool> HiddenRHSCheck = (c) =>
            {
                var positiveIndexes = new List<int>();
                var negativeIndexes = new List<int>();
                for (var i = 0; i < c.Count; i++)
                {
                    if (c[i] == 1)
                        positiveIndexes.Add(i);
                    else if (c[i] == -1)
                        negativeIndexes.Add(i);
                }
                return positiveIndexes.Count == 1 || !negativeIndexes.All(i => positiveIndexes.All(j => i < j));

            };


            return constraints.Where(CheckSingleType).Where(PositivesFlowCheck).Where(RHSLargeCheck).Where(HiddenRHSCheck).ToList();

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
