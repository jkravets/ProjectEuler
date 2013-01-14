using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Solvers;
namespace ProjectEuler.Problems
{
    public class Problem101
    {

        public void Solve()
        {

            var data = new List<CurvePoint>();

            
            data.Add(new CurvePoint(){x = 1, y = Generator(1)});
            data.Add(new CurvePoint(){x = 2, y = Generator(2)});
            var x = 2;

            long sumOfError = 0;

            //We first assume a constant curve
            if (data[1].y != data[0].y)
            {
                sumOfError += data[0].y;

                do
                {
                    var fit = new CurveFit(data);
                    
                    x++;
                    data.Add(new CurvePoint() { x = x, y = Generator(x) });
                    Console.WriteLine(fit.ToString());
                    if (fit[x] != data.Last().y)
                    {
                        sumOfError += fit[x];
                    }
                    else 
                    {
                        break;
                    }


                } while (true);

            }
            
            
            var answer = sumOfError;



        
        }


        public long Generator2(long n)
        {
            return  5*n*n*n * n * n;
        }
        public long Generator(long n)
        {
            return 1 - n + n*n - n*n*n + n*n*n*n - n*n*n*n*n + n*n*n*n*n*n - n*n*n*n*n*n*n + n*n*n*n*n*n*n*n - n*n*n*n*n*n*n*n*n + n*n*n*n*n*n*n*n*n*n;
        }



        public struct CurvePoint
        {
            public long x,y;
        }

        public class CurveFit
        {
            List<long> Coefficents { get; set; }


            public override string ToString()
            {
                var s = "";
                for (int i = 0; i < Coefficents.Count; i++)
                {
                    var power = Coefficents.Count - 1 - i;
                    var powerString = power > 0 ? "n^" + power : "";
                    powerString = power == 1 ? "n" : powerString;

                    var a = Math.Abs(Coefficents[i]);
                    var sign = Coefficents[i] > 0 ? "+" : "-";

                    if (i != 0 && a != 0)
                    {
                        s += sign + " ";
                    }

                    if (a != 0 && a != 1)
                    {                        
                        s += a + powerString + " ";
                    }
                    else if(a == 1 && powerString != "") 
                    {
                        s += powerString + " ";
                    }
                }
                return s;

            }

            private long Pow(long n, long power)
            {
                if (power == 0) return 1;
                long result = n;
                for (int i = 1; i < power; i++)
                {
                    result *=n;
                }
                return result;
            }

            private long Round(double n)
            {
                var dc = Math.Abs(Math.Ceiling(n) - n);
                var df = Math.Abs(Math.Floor(n) - n);
                if (dc < df)
                {
                    return (long)Math.Ceiling(n);
                }
                return (long)Math.Floor(n);
            }

            public long this[long n]
            {
                get
                {
                    long result = 0;
                    for (int i = 0; i < Coefficents.Count; i++)
                    {
                        var a = Coefficents[i];
                        result += a * Pow(n, Coefficents.Count - 1 - i);
                    }

                    return result;
                }
            }

            public CurveFit(List<CurvePoint> data)
            {
                var x = new DenseMatrix(data.Count, data.Count);

                for(int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        x[i, j] = Pow(data[i].x, data.Count - 1 - j);
                    }
                }

                var y = new DenseVector(data.Select(p => (double)p.y).ToArray());

                var coefficents = x.QR().Solve(y);



                Coefficents = coefficents.Select(c => Round(c)).ToList();

            }
        }

    }
}
