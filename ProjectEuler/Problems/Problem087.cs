using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem087
    {

        /*
         * 
         * N = a^2 + b^3 + c^4
         * 
         * a,c,b can only be prime numbers
         * 
         * Max N = 50
         * 
         * if b = 2, C = 2 then maximum number to search for is
         * 
         * 50 = c^2 = 7
         * 
         * 50,000,000 = c^2 = 7071, the highest prime you need to look to is 7071
         * there are 908 primes before this one.          
         * 
         * 
         * Assume we pick a value for a, then we know the smallest values we can pick for c is 2
         * 
         * N - a^2 - 8 = c^4
         * 
         * 
         * 
         */


        public void Answer()
        {
            var max = 50000000;
            var primes = Primes((int)Math.Ceiling(Math.Sqrt(max)));

            var numbers = new List<int>();

            for (int i = 0; i < primes.Count; i++)
            {
                var a = primes[i];

                //The biggest possible value of b
                var maxB = Math.Pow(max - Math.Pow(a, 2.0) - Math.Pow(2.0, 4.0), 0.33333333333);

                for (int j = 0; j < primes.Count && primes[j] < maxB; j++)
                {
                    var b = primes[j];
                    var maxC = Math.Pow(max - Math.Pow(a, 2.0) - Math.Pow(b, 3.0), 0.25);

                    for (int k = 0; k < primes.Count && primes[k] < maxC; k++)
                    {
                        var c = primes[k];

                        var number = (int)(Math.Pow(a, 2.0) + Math.Pow(b, 3.0) + Math.Pow(c, 4.0));
                        if (number < max)
                        {
                            numbers.Add(number);
                        }
                    }
                }
            }

            var count = numbers.Distinct().Count();

        }


        public List<int> Primes(int maximumValue)
        {
            var primes = new List<int>();

            for (int i = 2; i <= maximumValue; i++)
            {
                bool isPrime = true;
                for (int j = 2; j <= Math.Ceiling(i/2.0); j++)
                { 
                    var d = (double)i/(double)j;
                    isPrime = isPrime && ((d - Math.Floor(d)) != 0);
                }

                if (isPrime) {
                    primes.Add(i);
                }
            }

            return primes;
        }
    }
}
