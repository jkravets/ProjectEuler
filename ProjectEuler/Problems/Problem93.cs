using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NCalc;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace ProjectEuler.Problems
{
    public class Problem93
    {

        /*
         * 
         *  (a,b,c,d)
         *  Are numbers in a given set
         *  a < b < c < d
         *  
         *  (+,-,*,/) 
         *  Are operators
         *  
         *  A o1 B o2 C o3 D
         * 
         * 
         * At any given time there are only three possible operators in an equation.
         * There can be duplicates.
         * 
         * 
         * Brackets can also be used
         * 
         * (A o B)
         * 
         * (A o B o C)
         * ((A o B) o C)
         * (A o (B o C))
         * 
         * (A o B o C o D) -> three different ways to combine (A o B o C), other three for combining (B o C o D)
         * ((A o B) o (C o D)) 
         * (A o (B o C) o D)
         * 
         * For a 4 numbers there are 8 different bracket rules.
         * 
         * 
         * There are 126 permutations of different number sets
         * 24 permutations of operators
         * 8 different bracket combinations
         * 
         * 24192 different equations to try
         * 
         * 
         
         */
        public void Solve()
        {
            var n = 9;
            var chains = new List<Tuple<int,int,int,int,List<int>>>();
            for (int a = 1; a <= n; a++)
            {
                for (int b = a + 1; b <= n; b++)
                {
                    for (int c = b + 1; c <= n; c++)
                    {
                        for (int d = c + 1; d <= n; d++)
                        {
                            chains.Add(new Tuple<int,int,int,int,List<int>>(a,b,c,d,Chain(a, b, c, d)));
                        }
                    }
                }
            }




            var result = chains.OrderByDescending(c =>
            {
                var lastNumber = 1;
                foreach (var i in c.Item5)
                {
                    if (i != lastNumber) break;
                    lastNumber++;
                }
                return lastNumber;
            }).ToList();


        }

        public List<int> Chain(int a, int b, int c, int d)
        {
            var operators = new List<char>() { '+', '-', '*', '/' };


            const string format1 = "{0} {4} {1} {5} {2} {6} {3}";

            const string format2 = "({0} {4} {1} {5} {2}) {6} {3}";
            const string format3 = "(({0} {4} {1}) {5} {2}) {6} {3}";
            const string format4 = "({0} {4} ({1} {5} {2})) {6} {3}";

            const string format5 = "{0} {4} ({1} {5} {2} {6} {3})";
            const string format6 = "{0} {4} ({1} {5} ({2} {6} {3}))";
            const string format7 = "{0} {4} (({1} {5} {2}) {6} {3})";

            const string format8 = "({0} {4} {1}) {5} ({2} {6} {3})";
            const string format9 = "{0} {4} ({1} {5} {2}) {6} {3}";


            var numberSelections = new List<List<int>>();
            foreach (var selection in Permutations(new List<int>(){a,b,c,d}, 4, 0, false))
            {
                numberSelections.Add(selection);                
            }

            var operatorSelections = Permutations(operators, 3, 0, true);

            var engine = VsaEngine.CreateEngine();
            var result = new List<double>();
            foreach (var operatorSelection in operatorSelections)
            {                
                foreach (var numberSelection in numberSelections)
                {
                    Action<string> evaluateAndAdd = (format) =>
                    {
                        var equation = string.Format(format,
                         numberSelection[0],
                         numberSelection[1],
                         numberSelection[2],
                         numberSelection[3],
                         operatorSelection[0],
                         operatorSelection[1],
                         operatorSelection[2]);
                    
                        try
                        {
                            var value = Eval.JScriptEvaluate(equation, engine);
                            result.Add(System.Convert.ToDouble(value));
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            //Happens when dividing by zero
                        }
                    };

                    evaluateAndAdd(format1);
                    evaluateAndAdd(format2);
                    evaluateAndAdd(format3);
                    evaluateAndAdd(format4);
                    evaluateAndAdd(format5);
                    evaluateAndAdd(format6);
                    evaluateAndAdd(format7);
                    evaluateAndAdd(format8);
                    evaluateAndAdd(format9);

                }


            }
            return result.Where(i => i > 0 && (int)i - i == 0).Distinct().OrderBy(i => i).Select(i => (int)i).ToList();
        }


        public List<List<T>> Permutations<T>(List<T> list, int size, int depth = 0, bool allowDuplicates = false)
        {
            var permutations = new List<List<T>>();


            foreach (var a in list)
            {                
                var newList = list.ToList();
                if (!allowDuplicates)
                {
                    newList.Remove(a);
                }
                if (depth < size - 1)
                {
                    foreach (var smallerPerm in Permutations(newList, size, depth + 1, allowDuplicates))
                    {
                        var permuation = new List<T>() { a };
                        permuation.AddRange(smallerPerm);
                        permutations.Add(permuation);
                    }
                }
                else 
                {

                    var permuation = new List<T>() { a };
                    permutations.Add(permuation);
                }
            }

            return permutations;
        }

        
    }
}
