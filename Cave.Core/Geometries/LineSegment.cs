using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Cave.Core
{
    public class LineSegment : IGeometry
    {
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }

        public Geometry3D Model { get; protected set; }

        public LineSegment(Point3D startPoint, Point3D endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;

            var meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddCylinder(startPoint, endPoint, 0.2, 10);
                
            Model = meshBuilder.ToMesh();

        }
    }
}