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
            return 10; //TODO:
        }

        private double _diameter = -1;
    }
}