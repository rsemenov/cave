using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;

namespace Cave.Core
{
    public class CaveGraph
    {
        public Dictionary<string, CavePoint> PointsDict { get { return _pointsDict; } }
        private Dictionary<string, CavePoint> _pointsDict = new Dictionary<string, CavePoint>();

        public Dictionary<CavePoint, IList<CaveEdge>> AdjacencyList { get { return _adjacencyList; } }
        private Dictionary<CavePoint, IList<CaveEdge>> _adjacencyList = new Dictionary<CavePoint, IList<CaveEdge>>();
        
        private ILog _log = LogManager.GetCurrentClassLogger();

        public static CaveGraph ReadCave(string file)
        {
            return CaveReader.ReadCave(file);
        }

        public void ResolveCoordinates()
        {
            var root = PointsDict.Values.FirstOrDefault(point => point.Point.HasValue);
            if (root == null)
            {
                _log.ErrorFormat("Can not resolve coordinates. Root with specified coordinates not found");
                return;
            }
            Bfs(root);
            _log.InfoFormat("Coordinates resolved.");
        }

        public CaveBaseViewModel Render(CaveViewType type)
        {
            ResolveCoordinates();
            if (type == CaveViewType.Lines)
            {
                return new LineCaveViewModel(this);
            }
            return new TubeCaveViewModel(this);
        }

        public void AddEdge(CavePoint point, CaveEdge edge)
        {
            if (!AdjacencyList.ContainsKey(point))
            {
                AdjacencyList.Add(point, new List<CaveEdge>());
            }
            AdjacencyList[point].Add(edge);
        }

        public CavePoint GetPoint(string name, CaveGraph cave)
        {
            CavePoint p;
            if (cave.PointsDict.ContainsKey(name))
            {
                p = cave.PointsDict[name];
            }
            else
            {
                p = new CavePoint { Name = name };
                cave.PointsDict.Add(name, p);
            }

            return p;
        }

        private void Bfs(CavePoint root)
        {
            Queue<CavePoint> queue = new Queue<CavePoint>();
            queue.Enqueue(root);
            HashSet<string> used = new HashSet<string>();
            
            while(queue.Count>0)
            {
                var v = queue.Dequeue();
                used.Add(v.Name);

                if (!AdjacencyList.ContainsKey(v))
                {
                    continue;
                }

                foreach (var edge in AdjacencyList[v])
                {
                    if (!used.Contains(edge.EndPoint.Name))
                    {
                        edge.ResolveEndCoordinates();
                        used.Add(edge.EndPoint.Name);
                        queue.Enqueue(edge.EndPoint);
                    }
                }
            }
        }
    }
}