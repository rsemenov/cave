using System;
using System.Windows.Media.Media3D;
using Common.Logging;

namespace Cave.Core
{
    public class CaveEdge
    {
        public CavePoint StartPoint { get; set; }
        public CavePoint EndPoint { get; set; }
        public double Length { get; set; }
        public double Azimuth { get; set; }
        public double VerticalAngle { get; set; }

        public CaveEdge(CavePoint startPoint, CavePoint endPoint, double length, double azimuth, double vangle)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Length = length;
            Azimuth = azimuth;
            VerticalAngle = vangle;

            ResolveEndCoordinates();
            
        }

        private void ResolveEndCoordinates()
        {
            if(StartPoint.Point==null)
            {
                LogManager.GetCurrentClassLogger().WarnFormat("Cann't resovle EndPoint coordinates for edge {0}-{1}", StartPoint.Name, EndPoint.Name);
                return;
            }

            var p = Length*Math.Sin(VerticalAngle);
            var dx = p*Math.Sin(Azimuth);
            var dy = p*Math.Cos(Azimuth);
            var dz = Length*Math.Cos(VerticalAngle);

            EndPoint.Point = new Point3D()
                                 {
                                     X = StartPoint.Point.Value.X + dx,
                                     Y = StartPoint.Point.Value.Y + dy,
                                     Z = StartPoint.Point.Value.Z + dz
                                 };
        }
    }
}