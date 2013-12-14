using System.Collections.Generic;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Cave.Core
{
    public class Sphere : IGeometry
    {
        public Point3D Center { get; set; }
        public double Radius { get; set; }
        public Geometry3D Model { get; protected set; }
        public List<CutingPlane> CutingPlanes { get; private set; }

        public Sphere(Point3D center, double radius, IEnumerable<CutingPlane> cutingPlanes)
        {
            Center = center;
            Radius = radius;
            CutingPlanes  = new List<CutingPlane>();

            if (cutingPlanes != null)
            {
                CutingPlanes.AddRange(cutingPlanes);
            }

            var meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddSphere(center, radius, 25, 15);
            var m = meshBuilder.ToMesh();

            foreach (var plane3D in CutingPlanes)
            {
                Point3D p = plane3D.Point;
                Vector3D n = -1*plane3D.Vector;

                m = MeshGeometryHelper.Cut(m, p, n);
            }
            Model = m;
        }

    }
}