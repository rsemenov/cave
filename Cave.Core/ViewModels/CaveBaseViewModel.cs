using System.Collections.Generic;

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
            foreach (var kv in _caveGraph.AdjacencyList)
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
}