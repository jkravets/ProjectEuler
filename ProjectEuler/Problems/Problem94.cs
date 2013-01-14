using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ciloci.Flee.CalcEngine;
using System.Numerics;


namespace ProjectEuler.Problems
{
    public class Problem94
    {
        /*
         * 
         * A triangle is formed via the following string
         * 
         * (a,b,c)
         * 
         * The perimeter is
         * P = a + b + c
         * 
         * The area is
         * 
         * s = P/2
         * 
         * A = (s(s - a)(s - b)(s - c))^0.5
         * 
         * Order does not matter
         * 
         * Rule 1: a = b
         * 
         * Rule 2: c = a +- 1
         * 
         * Rule 3: the maximum perimeter = 1,000,000,000
         * 
         * 2a + c = 1,000,000,000
         * 
         * 2a + (a - 1)
         * 3a = 1,000,000,000 + 1
         * 
         * a <= 333,333,334
         * 
         * 
         * 
         * 
         */
        public void Solve()
        {
            double perimeterSum = 0;
            double max = 1000000000;
            double h, c = 0;

            
            for (double a = 1; a < (max / 3.0 + 1); a++)
            {
                c = a - 1;
                                
                if (c % 2 == 0 && c > 0)
                {
                    h = Math.Sqrt(a * a - (c * c) / 4);
                    var realH = (long)a * (long)a - ((long)c * ((long)c)) / 4;

                    if (Math.Floor(h) == h && realH == (h*h) && (3*a - 1) < max && h > 0)
                    {
                        perimeterSum += 3 * a - 1;
                        Console.WriteLine("a = " + a + " c = a - 1, h = " + h);
                    }
                }


                c = a + 1;
                if (c % 2 == 0 && c > 0)
                {
                    h = Math.Sqrt(a * a - (c * c) / 4);
                    var realH = (long)a*(long)a - ((long)c*((long)c))/4;
                    
                    if (Math.Floor(h) == h && realH == h*h && (3*a + 1) < max && h > 0)
                    {
                        perimeterSum += 3 * a + 1;
                        Console.WriteLine("a = " + a + " c = a + 1, h = " + h);
                    }
                }
            }



            var answer = perimeterSum;
        }

        /* http://www.alpertron.com.ar/METHODS.HTM
         * Case 1: a - 1
        
         * 
         * Ax^2 + Bxy + Cy^2 + Dx + Ey + F = 0
         * 
         * a^2 - 2h^2 - 2a - 2 = 0
         * A = 1, B = 0, C = -2, D = -2, E = 0, F = -2
         * 
         * a^2 - 2h^2 + 2a - 2 = 0
         * A = 1, B = 0, C = -2, D = 2, E = 0, F = -2
         
         
         */


        public long SolveCase1(long maxPerimeter)
        {
            
            // r*r + B*r*s + A*C*s*s = 1
            // r*r + -2*s*s = 1

            // 9 - 8
            // r = 3, s = 2 or -2
            
            

            var case1 = new GeneralQuadraticDiophatineEquationSolver()
            {
                A = 1,
                B = 0,
                C = -2,
                D = -2,
                E = 0,
                F = -2,
                r = 3,
                s = 2
            };


            long perimeterSum = 0;
            //First solution            
            long xprev = 17;
            long yprev = (long)Math.Sqrt(Math.Pow(xprev, 2.0) - Math.Pow((xprev-1)/2, 2.0));

            long xnext,ynext;

            while ((3*xprev - 1) < maxPerimeter)
            {
                case1.Next(xprev, yprev, out xnext, out ynext);

                
                Console.WriteLine((xprev) + "," + (xprev - 1));

                var a = xprev;
                var c = xprev - 1;
                var area = Math.Sqrt(c * c * (4 * a * a - c * c) / 16);
                Console.WriteLine(area);

                perimeterSum += 3 * xprev - 1;

                xprev = xnext;
                yprev = ynext;

            }

            return perimeterSum;

        }

        public long SolveCase2(long maxPerimeter)
        {
            var case2 = new GeneralQuadraticDiophatineEquationSolver()
            {
                A = 1,
                B = 0,
                C = -2,
                D = 2,
                E = 0,
                F = -2,
                r = 3,
                s = 2
            };

            //First solution     
            long perimeterSum = 0;
            long xprev = 5;
            long yprev = (long)Math.Sqrt(Math.Pow(xprev, 2.0) - Math.Pow((xprev + 1) / 2, 2.0));

            long xnext, ynext;

            while ((3*xprev + 1) < maxPerimeter)
            {
                case2.Next(xprev, yprev, out xnext, out ynext);

                perimeterSum += 3 * xprev + 1;
                Console.WriteLine(xprev + "," + (xprev + 1));

                var a = xprev;
                var c = xprev + 1;
                var area = Math.Sqrt(c * c * (4 * a * a - c * c) / 16);
                Console.WriteLine(area);

                xprev = xnext;
                yprev = ynext;

            }

            return perimeterSum;
        }


        public class GeneralQuadraticDiophatineEquationSolver
        {
            public long A { get; set; }
            public long B { get; set; }
            public long C { get; set; }
            public long D { get; set; }
            public long E { get; set; }
            public long F { get; set; }

            public long r { get; set; }
            public long s { get; set; }

            public void Next(long xprev, long yprev, out long xnext, out long ynext)
            {

                xnext = (r * r - A * C * s * s) * xprev - C * s * (2 * r + B * s) * yprev - C * D * s * s - E * r * s;
                ynext = A * s * (2 * r + B * s) * xprev + (r * r + 2 * B * r * s + (B * B - A * C) * s * s) * yprev + D * s * (r + B * s) - A * E * s * s;



            }


        }
        

    }
}
