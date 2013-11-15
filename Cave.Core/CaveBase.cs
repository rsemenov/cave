using System.Collections.Generic;
using Common.Logging;
using System.Globalization;

namespace Cave.Core
{
    public abstract class CaveBase
    {
        private List<IGeometry> _geometry = new List<IGeometry>();

        public IEnumerable<IGeometry> Geometry { get { return _geometry; } }

        protected CaveBase()
        {
        }

        protected CaveBase(IEnumerable<CaveEdge> segments)
        {
            foreach (var segment in segments)
            {
                AddSegment(segment.StartPoint, segment.EndPoint);
            }
        }

        public void AddSegment(CavePoint p1, CavePoint p2)
        {
            _geometry.Add(GetGeometry(p1,p2));
        }

        protected abstract IGeometry GetGeometry(CavePoint p1, CavePoint p2);
        
    }

    public class LineCave : CaveBase
    {
        public LineCave(IEnumerable<CaveEdge> segments) : base(segments)
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

    public class TubeCave : CaveBase
    {
        public TubeCave(IEnumerable<CaveEdge> segments) : base(segments)
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