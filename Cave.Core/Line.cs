using System.Windows;

namespace Cave.Core
{
    public class Line
    {
        public double K { get; set; }
        public double B { get; set; }

        public Line()
        { }

        public Line(Point p1, Point p2)
        {
            double a = p1.Y - p2.Y, b = p1.X - p2.X;
            double c = b*p1.Y - a*p1.X;
            K = a/b;
            B = c/b;
        }

        public Point Intersect(Line line)
        {
            double x = (line.B - this.B)/(this.K - line.K);
            double y = K*x + B;
            return new Point(x, y);
        }

        public Line Ortogonal(Point point)
        {
            double k = -1*1/(K+0.0000001);
            double b = point.Y - k*point.X;
            return new Line {K = k, B = b};
        }
    }
}