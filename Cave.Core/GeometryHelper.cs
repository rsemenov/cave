using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Cave.Core
{
    public static class GeometryHelper
    {
        public static double GetDiameterBy3Points(Point p1, Point p2, Point p3, out Point center)
        {
            var cp1 = new Point((p1.X + p2.X)/2, (p1.Y + p2.Y)/2);
            var cp2 = new Point((p3.X + p2.X)/2, (p3.Y + p2.Y)/2);
            var line1 = new Line(p1, p2);
            var line2 = new Line(p2, p3);
            var ort1 = line1.Ortogonal(cp1);
            var ort2 = line2.Ortogonal(cp2);
            center = ort1.Intersect(ort2);
            return GetDistance(p1, center);
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        
        public static Point3D GetDeltaPoint(Point3D a, Point3D b, double delta)
        {
            var c = new Point3D();
            var ab = b - a;
            c.X = a.X + (b.X - a.X) * delta / ab.Length;
            c.Y = a.Y + (b.Y - a.Y) * delta / ab.Length;
            c.Z = a.Z + (b.Z - a.Z) * delta / ab.Length;
            return c;
        }

        public static double GetDeltaDiameter(double r1, double r2, double len, double delta)
        {
            if (r1 < r2)
            {
                return GetDeltaDiameter(r2, r1, len, len - delta);
            }
            var x = len * r2 / (r1 - r2);
            var r = r1 * (1 - delta / (x + len));
            return r * 2;
        }

    }
}