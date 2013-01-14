using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem91
    {
        /*
         * 
         * There are 4 variables
         * 
         * (x1,y1) and (x2,y2)
         * 
         * The values can range from
         * 
         * 0 <= x1,y1,x2,y2 <= n
         * 
         * therefore the number of combinations of values is
         * 
         * n^4
         * 
         * given that n = 2, number of possible solutions = 16
         * 
         * given n = 50, solutions = 6250000, only 6 million combinations
         * 
         * 
         * Angle between two vectors
         * 
         * cos = (AxBx + AyBy)/AB
         * 
         * A = (Ax^2 + Ay^2)^1/2
         * B = (Bx^2 + By^2)^1/2
         * 
         * 
         * 
         * 
         * 
         */
        public void Solve()
        {
            var n = 50;
            var zero = new Point(){X = 0, Y = 0};
            var count = 0;

            for (var x1 = 0; x1 <= n; x1++)
            {
                for (var y1 = 0; y1 <= n; y1++)
                {
                    for (var x2 = 0; x2 <= n; x2++)
                    {
                        for (var y2 = 0; y2 <= n; y2++)
                        {
                            var A = new Point(){X = x1, Y = y1};
                            var B = new Point(){X = x2, Y = y2};

                            var v1 = new Vector() { A = zero, B = A };
                            var v2 = new Vector() { A = zero, B = B };
                            var v3 = new Vector() { A = A, B = B };
                            
                            if (v1.Angle(v2) == Math.PI/2.0 || v2.Angle(v3) == Math.PI/2.0 || v1.Angle(v3) == Math.PI/2.0)
                            {
                                count++;
                            }

                        }                        
                    }
                }
            }
            var rofl = count;

        }

        public void FasterSolution()
        {
            var n = 50;
            var count = 0;
            
            Func<int,int,double> Length = (I,J) =>
            {
                return Math.Sqrt(Math.Pow(I, 2.0) + Math.Pow(J, 2.0));
            };

            Func<double, double, double, double> Angle = (dot, len1, len2) =>
            {
                return Math.Abs(Math.Acos(dot/(len1*len2)));
            };

            for (var x1 = 0; x1 <= n; x1++)
            {
                for (var y1 = 0; y1 <= n; y1++)
                {
                    for (var x2 = 0; x2 <= n; x2++)
                    {
                        for (var y2 = 0; y2 <= n; y2++)
                        {
                            int I1 = x1, I2 = x2, I3 = x2 - x1;
                            int J1 = y1, J2 = y2, J3 = y2 - y1;

                            double dot1 = (I1 * I2 + J1 * J2), dot2 = (I1 * I3 + J1 * J3), dot3 = (I2 * I3 + J2 * J3);
                            double len1 = Length(I1, J1), len2 = Length(I2, J2), len3 = Length(I3, J3);

                            
                            if (Angle(dot1,len1,len2) == Math.PI / 2.0 || Angle(dot2,len1,len3) == Math.PI / 2.0 || Angle(dot3,len2,len3) == Math.PI / 2.0)
                            {
                                count++;
                            }

                        }
                    }
                }
            }

            var rofl = count;
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class Vector
        {
            public Point A { get; set; }
            public Point B { get; set; }

            public int I
            {
                get 
                {
                    return B.X - A.X;
                }
            }
            public int J
            {
                get 
                {
                    return B.Y - A.Y;
                }
            }
            public double Dot(Vector other)
            {
                return this.I * other.I + this.J * other.J;
            }
            public double Length
            {
                get 
                {
                    return Math.Sqrt(Math.Pow(I, 2.0) + Math.Pow(J, 2.0));
                }
            }

            public double Angle(Vector other)
            {
                return Math.Abs(Math.Acos(this.Dot(other)/(this.Length * other.Length)));
            }
        }
    }
}
