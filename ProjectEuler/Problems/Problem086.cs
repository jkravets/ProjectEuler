using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem086
    {
        public int Answer()
        {

            int integerCupoidsFound = 0;
            int integerCupoidsLookingFor = 1000000;
            int M = 0;
            
            do
            {
                M++;  
                for (int a = M; a <= M; a++)
                {
                    for (int b = 1; b <= a; b++)
                    {
                        for (int c = 1; c <= b; c++)
                        {
                            var shortest = Math.Sqrt(Math.Pow(a, 2.0) + Math.Pow((b + c), 2.0));
                            if (Math.Floor(shortest) - shortest == 0.0)
                            {
                                integerCupoidsFound++;
                            }
                        }
                    }
                }    
                
            } while (integerCupoidsFound < integerCupoidsLookingFor);

            return M;

        }
    }
}
