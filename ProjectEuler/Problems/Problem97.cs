using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem97
    {

        public void Solve()
        {

            //Try to compute the following number
            // 28433 * 2^7830457 + 1

            // Given a number
            /*
             * 
             * n = b - a
             * 
             * n = b*2 - a*2
             * 
             * Now make b = 1E6
             * 
             * n = 1000000 - a
             * 
             * 
             * 
             */
            


            long number = 28433;
            var exponent = 7830457;
            long powerRemover = (long)Math.Pow(10, 10);
            for (int i = 0; i < exponent; i++)
            {
                if (number > powerRemover)
                {
                    number -= powerRemover;
                }
                number *= 2;
            }

            number += 1;

            var answer = number;

            

            
        
        }

    }
}
