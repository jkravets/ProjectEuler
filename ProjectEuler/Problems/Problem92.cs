using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem92
    {

        /*
         * 
         * 44 -> 32 -> 13 -> 10 -> 1 -> 1
         * 
         * 44 = 4^2 + 4^2 = 32
         * 32 = 3^2 + 2^2 = 13
         * 
         */
        public void Solve()
        {
            //Zero will be ignored
            var results = new int[1 + 10000000];

            for (int i = 1; i < results.Length; i++)
            {
                results[i] = -1;
            }

            for (int i = 1; i < 600; i++)
            {
                var nextNumber = NextNumber(i);
                while (nextNumber != 1 && nextNumber != 89)
                {
                    nextNumber = NextNumber(nextNumber);
                }
                results[i] = nextNumber;
            }

            var length = results.Length;
            for (int i = 1; i < length; i++)
            {
                var nextNumber = NextNumber(i);
                results[i] = results[nextNumber]; 
            }


            var rofl = results.Where(i => i == 89);


        }

        public int NextNumber(int number)
        {
            double newNumber = 0;
            var numberString = number.ToString();
            for (int i = 0; i < numberString.Length; i++)
            {
                double n = char.GetNumericValue(numberString[i]);
                newNumber += n * n;
            }

            return (int)newNumber;
        }
    }
}
