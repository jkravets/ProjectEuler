using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem106
    {

       
        public void Solve()
        {

            var pairs = SubSetCombinations(4);


            var pairs7 = SubSetCombinations(7);

            var pairs12 = SubSetCombinations(12);

        }


        /*
         *
         * When n = 4,
         * 
         * how many different type of sub sets can we create
         * 
         * #A = 1, #B = 3,
         * #A = 1, #B = 2,
         * #A = 1, #B = 1,
         * #A = 2, #B = 2
         * 
         * !#A different ways to select the first set
         * 
         * Now whatever is left over is for B
         * 
         * #B Choose (n - #A)
         * 
         * 
         * 
         */
        public int SubSetCombinations(int n)
        {
            var combinations = 0;
            

            for (int A = 1; A <= n/2; A++)
            {
                for (int B = A; B <= (n - A); B++)
                {

                    Console.WriteLine("A: {0} choose {1}, B: {2} choose {3}", n, A, n - A, B);

                    combinations += Choose(n,A)*Choose(n - A, B);
                }
            }


            return combinations;
        
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
       

    }

}
