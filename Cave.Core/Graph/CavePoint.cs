using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Cave.Core
{
    public class CavePoint
    {
        public string Name { get; set; }

        public double[] Distances { get; set; }

        public Point3D? Point { get; set; }

        public double Diameter
        {
            get
            {
                if (_diameter == -1)
                {
                    _diameter = GetDiameter();
                }
                return _diameter;
            }
        }

        private double GetDiameter()
        {
            if (Distances != null)
            {
                var dd = Distances.Where(d => d >= 0);
                return dd.Sum() / dd.Count();
            }
            return 2; 

            var points = new List<Point>();
            if (Distances != null && Point.HasValue)
            {
                for (int i = 0; i < Distances.Length; i++)
                {
                    var dist = Distances[i];
                    if (points.Count < 3 && dist >= 0)
                    {
                        switch (i)
                        {
                            case 0: 
                                points.Add(new Point(Point.Value.Y - dist, Point.Value.Z));  
                                break;
                            case 1:
                                points.Add(new Point(Point.Value.Y + dist, Point.Value.Z));
                                break;
                            case 2:
                                points.Add(new Point(Point.Value.Y, Point.Value.Z + dist));
                                break;
                            case 3:
                                points.Add(new Point(Point.Value.Y, Point.Value.Z - dist));
                                break;
                        }
                    }
                }
                if (points.Count == 3)
                {
                    Point center;
                    var d = GeometryHelper.GetDiameterBy3Points(points[0], points[1], points[2], out center);
                    //UpdateCenter(center);
                    return d;
                }
            }
            if (Distances != null)
            {
                var dd = Distances.Where(d => d >= 0);
                return dd.Sum()/dd.Count();
            }
            return 1; 
        }

        private void UpdateCenter(Point center)
        {
            Point = new Point3D(Point.Value.X, center.X, center.Y);
        }

        private double _diameter = -1;
    }
}