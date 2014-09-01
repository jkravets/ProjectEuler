using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectEuler.Problems
{

    using Set = List<int>;
    using Group = Tuple<List<int>, List<int>>;

    public class GroupCompare : IEqualityComparer<Group>
    {
        private IntegerListCompare _setCompare { get; set; }
        public GroupCompare()
        {
            _setCompare = new IntegerListCompare();
        }

        public bool Equals(Group x, Group y)
        {
            return (_setCompare.Equals(x.Item1, y.Item1) && _setCompare.Equals(x.Item2, y.Item2)) ||
                   (_setCompare.Equals(x.Item2, y.Item1) && _setCompare.Equals(x.Item1, y.Item2));
        }

        public int GetHashCode(Group x)
        {
            return _setCompare.GetHashCode(x.Item1) + _setCompare.GetHashCode(x.Item2);
        }
    }


    public class Problem106
    {

       
        public void Solve()
        {
            var n = 12;
            var groups = new List<Group>();
            for (var i = 2; i <= n / 2; i++)
            {
                var subsets = DisjointSubsets(n, i).Where(NeedToCompare);
                groups.AddRange(subsets.ToList());
            
            }
            
            foreach (var group in groups)
            {
                Console.WriteLine("{0},{1}", group.Item1.PrettyPrint(), group.Item2.PrettyPrint());
            }

            Console.WriteLine("{0}", groups.Count);

            /*
            foreach(var group in groups)
            {
                Console.WriteLine("{0}, {1}", group.Item1.PrettyPrint(), group.Item2.PrettyPrint());
            }
             */
        }
        //Out of all the
        public bool NeedToCompare(Group group)
        {
            var leftHeavy = true;
            var rightHeavy = true;
            
            //Check if the indexes do not match
            for (var i = 0; i < group.Item1.Count; i++)
            {
                if (group.Item1[i] >= group.Item2[i])
                {
                    leftHeavy = false;
                }
                if (group.Item1[i] <= group.Item2[i])
                {
                    rightHeavy = false;
                }
            }

            return !leftHeavy && !rightHeavy;
        }

        public List<Set> Combinations(Set things, int size)
        {
            var combinations = things.Combinations(size);
            foreach (var c in combinations)
            {
                c.Sort();
            }
            combinations = combinations.Distinct(new IntegerListCompare()).ToList();
            return combinations;
        }

        public List<Group> DisjointSubsets(int n, int size)
        {
            var indexes = new Set();
            for (var i = 0; i < n; i++)
            {
                indexes.Add(i);
            }

            //Generate the left side first
            var groups = new List<Group>();
            var leftSide = Combinations(indexes, size);

            foreach (var A in leftSide)
            {
                var rightSide = Combinations(indexes.Except(A).ToList(), size);
                foreach (var B in rightSide)
                {
                    if (A.Count == B.Count)
                    {
                        groups.Add(new Group(A, B));
                    }
                }
            }
            groups = groups.Distinct(new GroupCompare()).ToList();
            return groups;
        }



        /**
         * Generate all possible equal sized sets
         * 
         * 0, 1, 2, 3
         * 
         * 0, 1
         * 0, 2
         * 0, 3
         * 1, 2
         * 1, 3
         * 2, 3
         * 
         * Compare the left and right hand side
         * 
         * 
         * 
         * 
         * 
         */

        /**
         * a < b < c < d
         * 1 < 2 < 3 < 4
         * 
         * Only care when the sets are equal size
         * 
         * 4 choose 2 = 6 combinations on the left
         * 2 choose 2 = 1 combination on the right
         * 
         * possible sets that could be tested
         * 
         * 5 choose 2 = 10
         * 3 choose 2 = 3
         * => 30 possible sets
         * 
         * 
         * 13 24
         * 14 23
         */

        public int EqualSets(int n) 
        {
            int equal = 0;
            AChoices(n, ref equal);
            return equal;
        }
        public void AChoices(int n, ref int equal)
        {
            for (int c = 1; c < n; c++)
            {
                Console.WriteLine("A: {0} choose {1}", n, c);
                for (int c2 = 1; c2 <= n - c; c2++)
                {
                    if (c == c2 && c != 1 && c2 != 1)
                    {
                        var choices = Choose(n, c);
                        var choices2 = Choose(n - c, c2);

                        Console.WriteLine("A: {0} choose {1} = {2}, B: {3} choose {4} = {5}", n, c, choices, n - c, c2, choices2);

                    }
                }            
            }
        }
        public int Choose(int n, int c)
        {
            return Factorial(n) / (Factorial(c) * (Factorial(n - c)));
        }
        public int Factorial(int n)
        {            
            if(n == 1 || n == 0) return 1;
            return n*Factorial(n-1);
        }


        public int Combinations(int n, int k, ref int equality) 
        {
            if (k == 0 || n == 0) return 1;
            Console.WriteLine("{0} choose {1}", n, k);
            if (k > 0)
            {
                return Combinations(n, k - 1, ref equality) * (n - k + 1) / k;
            }
            if (k < n)
            {
                return Combinations(n - 1, k, ref equality) * n / (n - k);
            }
            return 1;
        }
       

    }

}
