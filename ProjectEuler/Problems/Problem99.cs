using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem99
    {
        public void Solve()
        {


            var lines = new List<ExpLine>();
            var dirtyLines = File.ReadAllText(@"Assets\\base_exp.txt").Split('\n').ToList();
            for(int i = 0; i < dirtyLines.Count; i++)
            {
                var components = dirtyLines[i].Split(',');

                lines.Add(new ExpLine(){
                    Line = i+1,
                    Base = Int32.Parse(components[0]),
                    Power = Int32.Parse(components[1])
                });
            }
            
            var expComparer = new ExpLineComparer();
            var t1 = new ExpLine() { Base = 2, Power = 11 };
            var t2 = new ExpLine() { Base = 3, Power = 7 };
            var result = expComparer.Compare(t1, t2);

            lines.Sort(expComparer);

            var answer = lines.Last();


        }

        public class ExpLine
        {
            public int Base { get; set; }
            public int Power { get; set; }
            public int Line { get; set; }            

        }

        /*
         * 
         * which is bigger
         * 
         * b1^p1 or b2^p2
         * 
         * We know that
         * 
         * logb1(b1^p1) = b1
         * 
         * logb(m^n) = n*logb(m)
         * 
         * b^(n*logb(m))
         * 
         * Therefore we know that
         * 
         * b1^p1 = e^(p1*ln(b1))
         * 
         * So now we can convert both numbers to that base
         * 
         * 
         * 
         * 
         */
        public class ExpLineComparer : IComparer<ExpLine>
        {

            public int Compare(ExpLine x, ExpLine y)
            {
                var p1 = x.Power * Math.Log10(x.Base);
                var p2 = y.Power * Math.Log10(y.Base);

                //Given this we know have that
                //10 ^ p1 = x.Base^x.Power
                //10 ^ p2 = y.Base^y.Power
                //So just look at p1 and p2

                return p1.CompareTo(p2);


            }
        }
    }
}
