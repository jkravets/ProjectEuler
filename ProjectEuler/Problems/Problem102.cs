using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem102
    {
        public void Solve()
        {
            var dirtyLines = File.ReadAllText(@"Assets\\triangles.txt").Split('\n').ToList();
            var triangles = new List<Triangle>();

            foreach (var dLine in dirtyLines)
            {
                var c = dLine.Split(',').ToList();
                if (c.Count == 6)
                {
                    triangles.Add(new Triangle()
                    {
                        A = new Point() { x = Double.Parse(c[0]), y = Double.Parse(c[1]) },
                        B = new Point() { x = Double.Parse(c[2]), y = Double.Parse(c[3]) },
                        C = new Point() { x = Double.Parse(c[4]), y = Double.Parse(c[5]) }
                    });
                }
            }


            var sum = 0;
            var zero = new Point() { x = 0, y = 0 };
            foreach (var triangle in triangles)
            {
                sum += triangle.Contains(zero) ? 1 : 0;
            }
            var answer = sum;


        }
    }


    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        public double Distance(Point other)
        {
            return Math.Sqrt(Math.Pow(x - other.x, 2.0) + Math.Pow(y - other.y, 2.0));
        }

    }

    public class SlopeLine
    {
        public double m { get; private set; }
        public double b { get; private set; }

        public SlopeLine(Point p1, Point p2)
        {
            m = (p2.y - p1.y) / (p2.x - p1.x);
            b = p1.y - m * p1.x;
        }

        public double Y(double x)
        {
            return m * x + b;
        }
        public double X(double y)
        {
            return (y - b) / m;
        }

        public Point Intersection(SlopeLine other)
        { 
            //y = m1*x + b1
            //y = m2*x + b2

            //m1*x + b1 = m2*x + b2
            //x(m1 - m2) = b2 - b1
            //x = (b2 - b1)/(m1 - m2)

            var p = new Point();
            p.x = (other.b - this.b) / (this.m - other.m);
            p.y = this.m * p.x + this.b;

            return p;
        }

        public Point Shortest(Point other)
        { 
            // y = mx + b
            // perpendicular line is
            
            // y = (1/m)x + b2
            // 
            // mx + b = (1/m)x + b2

            // mx - (1/m)x = b2 - b
            // x(m - 1/m) = b2 - b
            // x = (b2 - b)/(m - 1/m)



            //o.y = (1/m)o.x + b2
            //b2 = o.y - (1/m)o.x

            // s.x = (o.y - (1/m)o.x)/(m - 1/m)

            var m2 = 1.0 / m;
            var b2 = other.y - m2 * other.x;

            var shortest = new Point();
            shortest.x = (other.y - m2 * other.x) / (m - m2);
            shortest.y = m * shortest.x + b;

            return shortest;
            
        }
    }
    public class Triangle
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        
        /* 
         * A = (x1,y1)
         * B = (x2,y2)
         * C = (x3,y3)
         * 
         * There are three line equations
         * 
         * AB => y = mx + b
         * BC => y = mx + b
         * CA => y = mx + b
         * 
         * Also find
         * AO
         * BO
         * CO 
         */
        public bool Contains(Point o)
        {
            var AB = new SlopeLine(A, B);
            var BC = new SlopeLine(B, C);
            var CA = new SlopeLine(C, A);

            var largestShortest = Math.Max(AB.Shortest(C).Distance(C), BC.Shortest(A).Distance(A));
            largestShortest = Math.Max(largestShortest, CA.Shortest(B).Distance(B));

            //Now check for the contains
            var e = 0.000001;
            return false;
            
        }
    }
}
