using System.Collections.Generic;
using Common.Logging;

namespace Cave.Core
{
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
}