using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem104
    {

        public void Solve()
        {

            var n1 = new LargeNaturalNumber(556312);
            var n2 = new LargeNaturalNumber(556312);

            var n3 = n1.Add(n2);

            var test = Fib(2749);
            var rofl = test.Last9();
            var rofl2 = test.First9();


            var fibPrev = new LargeNaturalNumber(1);
            var fibPrevPrev = new LargeNaturalNumber(1);

            for (int i = 3; i < 10000000; i++)
            {
                var result = fibPrev.Add(fibPrevPrev);
                fibPrevPrev = fibPrev;
                fibPrev = result;                

                var isPanLast = IsPanDigital(result.Last9());
                var isPanFirst = IsPanDigital(result.First9());
                if (isPanLast && isPanFirst)
                {
                    Console.WriteLine("Pandigital in last and first is: " + i);
                    var answer = result;
                }
                else if (isPanLast)
                {
                    Console.WriteLine("Pandigital last is: " + i);
                }
                else if (isPanFirst)
                {
                    Console.WriteLine("Pandigital first is: " + i);
                }

                

            
            
            }
            

        }

        //How to check if a 9 digit number is pandigital
        public bool IsPanDigital(string s)
        {
            for (int i = 1; i <= 9; i++)
            {
                if (!s.Contains(i.ToString()[0])) 
                {
                    return false;
                }
            }
            return true;
        }

        //Fibonaci
        private Dictionary<long, LargeNaturalNumber> FibMap = new Dictionary<long, LargeNaturalNumber>();

        private LargeNaturalNumber Fib(long n)
        {
            if (n == 1 || n == 2) return new LargeNaturalNumber(1);

            if (FibMap.ContainsKey(n)) return FibMap[n];

            var result = Fib(n - 1).Add(Fib(n - 2));
            FibMap[n] = result;
            return result;
        }


        public class LargeNaturalNumber
        {
            //Used to control the coefficent growth
            private long e = (long)Math.Pow(10,9);

            public int MaxCoefficent = 0;
            public long[] Coefficents = new long[100000];

            public LargeNaturalNumber()
            {
            }
            public LargeNaturalNumber(long n)
            {
                var d = n % e;
                Coefficents[0] = d;
                MaxCoefficent = 0;

                n = n - d;

                while (n != 0)
                {
                    var factorBase = (int)Math.Floor(Math.Log(n, e));
                    if (factorBase > 0)
                    {
                        Coefficents[factorBase] = n / (long)Math.Pow(e, factorBase);
                    }
                    MaxCoefficent = Math.Max(factorBase,MaxCoefficent);
                    n = n - (long)Math.Pow(e, factorBase) * Coefficents[factorBase];
                }                
            }

            public long LongValue()
            {
                long sum = 0;
                for (int i = 0; i <= MaxCoefficent; i++)
                {
                    sum += (long)(Coefficents[i] * (long)Math.Pow(e, i));
                }
                return sum;

            }

            public string Last9()
            {
                var s = "";
                for (int i = 0; i <= MaxCoefficent; i++)
                {
                    s += Coefficents[i].ToString();
                    if (s.Length >= 9)
                    {
                        break;
                    }
                }
                return s.Substring(0, Math.Min(s.Length,9)).ToString();
            
            }

            public string First9()
            {
                var s = "";
                for (int i = MaxCoefficent; i >= 0; i--)
                {
                    s += Coefficents[i].ToString();
                    if (s.Length >= 9)
                    {
                        break;
                    }
                }
                return s.Substring(0, Math.Min(s.Length, 9)).ToString();
            }

            public override string ToString()
            {
                var s = "x = 0";
                for (int i = 0; i <= MaxCoefficent; i++)
                {

                    s += " + ";
                    s += i > 0 ? Coefficents[i].ToString() + "e^" + i : Coefficents[i].ToString();
                }
                return s;
            }


            //Adds two large natural numbers together
            public LargeNaturalNumber Add(LargeNaturalNumber n2)
            {
                var newNumber = new LargeNaturalNumber();
                newNumber.MaxCoefficent = Math.Max(MaxCoefficent, n2.MaxCoefficent);
                var carry = 0;
                for (int i = 0; i <= newNumber.MaxCoefficent; i++)
                {
                    var ci = Coefficents[i] + n2.Coefficents[i] + carry;

                    if (ci >= e)
                    {
                        newNumber.Coefficents[i] = ci - e;
                        carry = 1;
                    }
                    else
                    {
                        newNumber.Coefficents[i] = ci;
                        carry = 0;
                    }
                }

                if (carry == 1)
                {
                    newNumber.MaxCoefficent++;
                    newNumber.Coefficents[newNumber.MaxCoefficent] = 1;
                }

                return newNumber;
            }


        }

    }
}
