using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using System.Globalization;
using System.Windows.Media.Media3D;
using System;

namespace Cave.Core
{
    public abstract class CaveBaseViewModel
    {
        protected CaveGraph _caveGraph;
        protected List<IGeometry> _geometry = new List<IGeometry>();

        public IEnumerable<IGeometry> Geometry { get { return _geometry; } }

        protected CaveBaseViewModel(CaveGraph caveGraph)
        {
            _caveGraph = caveGraph;
            Build();
        }

        protected virtual void Build()
        {
            foreach (var kv in _caveGraph.CaveStruct)
            {
                foreach (var caveEdge in kv.Value)
                {
                    _geometry.AddRange(GetGeometry(kv.Key, caveEdge.EndPoint));
                }
            }
            _geometry.AddRange(GetAdditionalGeometry());
        }

        protected abstract IEnumerable<IGeometry> GetGeometry(CavePoint p1, CavePoint p2);
        protected abstract IEnumerable<IGeometry> GetAdditionalGeometry();
        
    }

    public class LineCaveViewModel : CaveBaseViewModel
    {
        public LineCaveViewModel(CaveGraph caveGraph) : base(caveGraph)
        {}

        protected override IEnumerable<IGeometry> GetGeometry(CavePoint p1, CavePoint p2)
        {
            if (p1.Point != null && p2.Point != null)
            {
                return new[] {new LineSegment(p1.Point.Value, p2.Point.Value)};
            }

            LogManager.GetCurrentClassLogger().ErrorFormat("Can not build LineSegment one of the points has empty coordinates. p1Name={0}, p2Name={1}", p1.Name, p2.Name);
            return null;
        }

        protected override IEnumerable<IGeometry> GetAdditionalGeometry()
        {
            return new IGeometry[0];
        }
    }

    public class TubeCaveViewModel : CaveBaseViewModel
    {
        Dictionary<CavePoint, List<CavePoint>> points = new Dictionary<CavePoint, List<CavePoint>>();

        public TubeCaveViewModel(CaveGraph caveGraph) : base(caveGraph)
        {}

        protected override IEnumerable<IGeometry> GetGeometry(CavePoint p1, CavePoint p2)
        {
            if (p1.Point != null && p2.Point != null)
            {
                var geoms = new List<IGeometry>();
                geoms.Add(new TubeSegment(p1.Point.Value, p2.Point.Value, p1.Diameter, p2.Diameter));
                
                if (!points.ContainsKey(p1))
                {
                    points.Add(p1, new List<CavePoint>());
                }
                points[p1].Add(p2);

                if (!points.ContainsKey(p2))
                {
                    points.Add(p2, new List<CavePoint>());
                }
                points[p2].Add(p1);

                return geoms;
            }
            LogManager.GetCurrentClassLogger().ErrorFormat("Can not build TubeSegment one of the points has empty coordinates. p1Name={0}, p2Name={1}", p1.Name, p2.Name);
            return null;
        }

        protected override IEnumerable<IGeometry> GetAdditionalGeometry()
        {
            var geoms = new List<IGeometry>();

            foreach (var p in points.Where(x=>x.Value.Count > 1))
            {
                var a = p.Key;
                var delta = 0.1*a.Diameter;
                var r1 = 0.9*a.Diameter/2; //GeometryHelper.GetDeltaDiameter(a.Diameter / 2, b0.Diameter / 2, len, delta) / 2;

                var rs = Math.Sqrt(delta*delta + r1*r1);

                geoms.Add(new Sphere(a.Point.Value, rs, //a.Diameter / 2, 
                    points[p.Key].Select(b =>
                        {
                            var da = GeometryHelper.GetDeltaPoint(a.Point.Value, b.Point.Value, 0.1*a.Diameter);
                            var db = GeometryHelper.GetDeltaPoint(b.Point.Value, a.Point.Value, 0.1*b.Diameter);
                            return new CutingPlane(da, db - da);
                        })));
            }

            return geoms;
        }
    }
}