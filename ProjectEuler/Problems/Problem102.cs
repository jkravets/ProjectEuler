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
                    triangles.Last().InitializeSlopes();
                }
            }


            /* OLD
            var p = new Point() { x = 1.0, y = 1.0 };
            var p2 = new Point() { x = 1.0, y = 1.0 };
            var sum = 0;
            foreach (var triangle in triangles)
            {
                if (triangle.A.Equals(triangle.B) || triangle.A.Equals(triangle.C) || triangle.B.Equals(triangle.C))
                {
                    var what = "";
                }

                if (triangle.A.Equals(zero) || triangle.B.Equals(zero) || triangle.C.Equals(zero))
                {
                    var what = "";
                }



                if (triangle.AB.IsVertical || triangle.BC.IsVertical || triangle.CA.IsVertical)
                {
                    var what = "";
                }

                if (triangle.AB.IsHorizontal || triangle.BC.IsHorizontal || triangle.CA.IsHorizontal)
                {
                    var what = "";
                }


                sum += triangle.Contains(zero) ? 1 : 0;
            }
            */


            var zero = new Point() { x = 0, y = 0 };
            var zeroTriangles = triangles.Where(t => t.Contains(zero)).ToList();
            
            //The trick is some of the triangles are duplicates !!!
            var distinctZeroTriangles = zeroTriangles.Distinct(new TriangleEqualityComparer());


            var answer = distinctZeroTriangles.Count();
            var rofl = answer;
        }
    }

    public class TriangleEqualityComparer : IEqualityComparer<Triangle>
    {

        public bool Equals(Triangle x, Triangle y)
        {
            // ABC => ABC
            var t1 = x.A.Equals(y.A) && x.B.Equals(y.B) && x.C.Equals(y.C);
            
            // ABC => BCA
            var t2 = x.A.Equals(y.B) && x.B.Equals(y.C) && x.C.Equals(y.A);
            
            // ABC => CAB
            var t3 = x.A.Equals(y.C) && x.B.Equals(y.A) && x.C.Equals(y.B);
            
            // ABC => CBA
            var t4 = x.A.Equals(y.C) && x.B.Equals(y.B) && x.C.Equals(y.A);

            return t1 || t2 || t3 || t4;


        }

        public int GetHashCode(Triangle obj)
        {
            return obj.A.GetHashCode() + obj.B.GetHashCode() + obj.C.GetHashCode();
        }
    }
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }

        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }
        public override string ToString()
        {
            return "(" + x + " , " + y + ")";
        }

        public override bool Equals(object obj)
        {
            var other = obj as Point;
            if (other != null)
            {
                return other.x == this.x && other.y == this.y;
            }
            return base.Equals(obj);
        }
    }

    public class SlopeLine
    {
        public Point P1 { get; private set; }
        public Point P2 { get; private set; }
        public double m { get; private set; }
        public double b { get; private set; }
        public bool IsVertical
        {
            get 
            {
                return P1.x == P2.x;
            }
        }

        public bool IsHorizontal
        {
            get
            {
                return P1.y == P2.y;
            }
        }


        public SlopeLine(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
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

        public double MinBoundX
        {
            get 
            {
                return Math.Min(P1.x, P2.x);
            }
        }

        public double MaxBoundX
        {
            get
            {
                return Math.Max(P1.x, P2.x);
            }
        }

        public double MinBoundY
        {
            get
            {
                return Math.Min(P1.y, P2.y);
            }
        }

        public double MaxBoundY
        {
            get
            {
                return Math.Max(P1.y, P2.y);
            }
        }

    }
    public class Triangle
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }

        public SlopeLine AB { get; set; }
        public SlopeLine BC { get; set; }
        public SlopeLine CA { get; set; }

        public void InitializeSlopes()
        {
            AB = new SlopeLine(A, B);
            BC = new SlopeLine(B, C);
            CA = new SlopeLine(C, A);
        }

        public override string ToString()
        {
            return A + " " + B + " " + C;
        }

        public bool Contains(Point o)
        {
            var yMax = Math.Max(Math.Max(A.y, B.y), C.y);
            var yMin = Math.Min(Math.Min(A.y, B.y), C.y);

            var xMax = Math.Max(Math.Max(A.x, B.x), C.x);            
            var xMin = Math.Min(Math.Min(A.x, B.x), C.x);

            
            //Only consider values that are valid
            var validValues = new List<double>();
            
            Func<double, double, double, bool> boundCheck = (min, v, max) =>
            {
                return min <= v && v <= max;
            };

            if (!boundCheck(xMin, o.x, xMax) || !(boundCheck(yMin, o.y, yMax))) 
            {
                return false;
            }

            Action<SlopeLine> checkValid = (s) =>
            {
                var yS = s.Y(o.x);
                if (boundCheck(s.MinBoundY, yS, s.MaxBoundY))
                {
                    validValues.Add(yS);
                }
            };

            checkValid(AB);
            checkValid(BC);
            checkValid(CA);

                        
            var yMinBound = validValues.Min();
            var yMaxBound = validValues.Max();

            return boundCheck(yMinBound, o.y, yMaxBound);

        }
    }
}
