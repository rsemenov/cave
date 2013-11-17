using System;
using System.Windows;

namespace Cave.Core
{
    public static class GeometryHelper
    {
        public static double GetDiameterBy3Points(Point p1, Point p2, Point p3)
        {
            var cp1 = new Point((p1.X + p2.X)/2, (p1.Y + p2.Y)/2);
            var cp2 = new Point((p3.X + p2.X)/2, (p3.Y + p2.Y)/2);
            var line1 = new Line(p1, p2);
            var line2 = new Line(p2, p3);
            var ort1 = line1.Ortogonal(cp1);
            var ort2 = line2.Ortogonal(cp2);
            var center = ort1.Intersect(ort2);
            return GetDistance(p1, center);
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
            

    }
}