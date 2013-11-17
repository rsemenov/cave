using System.Collections.Generic;
using Common.Logging;
using System.Globalization;

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
                    _geometry.Add(GetGeometry(kv.Key, caveEdge.EndPoint));
                }
            }
        }
        
        protected abstract IGeometry GetGeometry(CavePoint p1, CavePoint p2);
        
    }

    public class LineCaveViewModel : CaveBaseViewModel
    {
        public LineCaveViewModel(CaveGraph caveGraph) : base(caveGraph)
        {}

        protected override IGeometry GetGeometry(CavePoint p1, CavePoint p2)
        {
            if (p1.Point != null && p2.Point != null)
            {
                return new LineSegment(p1.Point.Value, p2.Point.Value);
            }

            LogManager.GetCurrentClassLogger().ErrorFormat("Can not build LineSegment one of the points has empty coordinates. p1Name={0}, p2Name={1}", p1.Name, p2.Name);
            return null;
        }
    }

    public class TubeCaveViewModel : CaveBaseViewModel
    {
        public TubeCaveViewModel(CaveGraph caveGraph) : base(caveGraph)
        {}

        protected override IGeometry GetGeometry(CavePoint p1, CavePoint p2)
        {
            if (p1.Point != null && p2.Point != null)
            {
                return new TubeSegment(p1.Point.Value, p2.Point.Value, p1.Diameter, p2.Diameter);
            }
            LogManager.GetCurrentClassLogger().ErrorFormat("Can not build TubeSegment one of the points has empty coordinates. p1Name={0}, p2Name={1}", p1.Name, p2.Name);
            return null;
        }
    }
}