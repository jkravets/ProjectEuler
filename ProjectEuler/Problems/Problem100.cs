using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem100
    {

        /*
         * 
         * Box is of size n
         * 
         * b is number of blue discs
         * 
         * Therefore we are looking for all integer solutions to the equation
         * 
         * (b(b-1))/(n(n-1)) = 0.5
         * 
         * (b^2 - b)/(n^2 - n) = 0.5
         * 
         * 2(b^2 - b)/(n^2 - n) - 1 = 0
         * 
         * 
         */

        //Other ways of looking at the problem

        /*
         * b is divisible by n
         * 
         * The top must be divisible by one of the number
         * 
         * (b(b-1)) % n == 0
         * 
         * or
         * 
         * (b(b-1)) % (n-1) == 0
         * 
         * Then the solution becomes
         * 
         * 
         * What is the smallest possible value for b?
         * 
         * b = n - a
         * 
         * b is smaller than a by a certain amount
         * 
         * (n - a)(n - a - 1)
         * n^2 - na - n - an + a^2 + a
         * 
         *  (n^2 - 2na + a^2 - n + a)/(n^2 - n) = 0.5
         *  
         * Solve for a
         * 
         * n^2 - 2na + a^2 - n + a = 0.5(n^2 - n)
         * n^2 - 2na + a^2 - n + a - 0.5n^2 + 0.5n = 0
         * 
         * 0.5n^2 - 2na + a^2 - 0.5n + a = 0
         * 
         * n^2 - 4na + 2a^2 - n + 2a = 0
         * 
         * 
         * given n = 21
         * and a = 6
         * 
         * 
         * 
         */

        public void Solve()
        {
            var afterValue = Math.Pow(10, 12);

            long prevN = 1, prevA = 0, nextN = 0, nextA = 0;

            do
            {
                Console.WriteLine(prevN + "," + (prevN - prevA));
                NextStep(prevN, prevA, out nextN, out nextA);
                
                if (nextN > afterValue)
                {
                    break;
                }

                prevN = nextN;
                prevA = nextA;

            } while (true);


            var answer = nextN - nextA;

        }

        /*
         * 
         * Using this solver online
         * http://www.alpertron.com.ar/QUAD.HTM
         * 
         * and the equation
         * 
         * n^2 - 4na + 2a^2 - n + 2a = 0
         * 
         * we get the following properties
         * 
         */

        public void NextStep(long prevN, long prevA, out long nextN, out long nextA)
        {
            nextN = 7 * prevN - 4 * prevA - 3;
            nextA = 2 * prevN - 1 * prevA - 1;            
        }



    }
}
