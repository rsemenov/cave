using System.Collections.Generic;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Cave.Core
{
    public class TubeSegment :IGeometry
    {
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public double StartRadius { get; set; }
        public double EndRadius { get; set; }

        public Geometry3D Model { get; protected set; }

        public TubeSegment(Point3D startPoint, Point3D endPoint, double d1, double d2)
        {
            var meshBuilder = new MeshBuilder(false, false);
            var points = new List<Point3D>() { startPoint, endPoint };
            var diameters = new [] { d1, d2 };
            meshBuilder.AddTube(points, new double[] { 0, 0 }, diameters, 20, false);

            Model = meshBuilder.ToMesh();
        }
    }
}