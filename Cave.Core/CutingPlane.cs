using System.Windows.Media.Media3D;

namespace Cave.Core
{
    public class CutingPlane
    {
        public Point3D Point { get; set; }
        public Vector3D Vector { get; set; }

        public CutingPlane(Point3D point, Vector3D vector)
        {
            Point = point;
            vector.Normalize();
            Vector = vector;
        }
    }
}