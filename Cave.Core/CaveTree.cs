using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;

namespace Cave.Core
{
    public class CaveTree
    {
        protected Dictionary<string, CavePoint> pointsDict = new Dictionary<string, CavePoint>();
        protected Dictionary<CavePoint, IList<CaveEdge>> dict = new Dictionary<CavePoint, IList<CaveEdge>>();

        public static CaveTree ReadCave(string csvFilePath)
        {
            var lines = File.ReadAllLines(csvFilePath);
            var tree = new CaveTree();

            for( int i=1; i<lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                CavePoint p1;
                if (tree.pointsDict.ContainsKey(parts[0])) { p1 = tree.pointsDict[parts[0]]; }
                else {
                    p1 = new CavePoint {Name = parts[0]};
                    tree.pointsDict.Add(parts[0], p1);
                }

                p1.Distances = parts.Where((p, ind) => ind > 4 && ind <= 8).Select(p =>
                {
                    double d;
                    return double.TryParse(p, out d) ? d : -1;
                }).ToArray();

                double x, y, z;
                if(double.TryParse(parts[9], out x) && double.TryParse(parts[10], out y) && double.TryParse(parts[11], out z))
                {
                    p1.Point = new Point3D(x, y, z);
                }

                CavePoint p2;
                if (tree.pointsDict.ContainsKey(parts[1]))
                {
                    p2 = tree.pointsDict[parts[1]];
                }
                else
                {
                    p2 = new CavePoint { Name = parts[1] };
                    tree.pointsDict.Add(parts[1], p2);
                }

                double len, azimuth, vangel;
                if(double.TryParse(parts[4], out len) && double.TryParse(parts[4], out azimuth) && double.TryParse(parts[4], out vangel))
                {
                    var edge = new CaveEdge(p1, p2, len, azimuth, vangel);
                    if(!tree.dict.ContainsKey(p1))
                    {
                        tree.dict.Add(p1, new List<CaveEdge>());
                    }
                    tree.dict[p1].Add(edge);
                }
                else
                {
                    LogManager.GetCurrentClassLogger().ErrorFormat("Cannot parse input data");
                    throw new ArgumentException("Cannot parse input data");
                }
            }

            return tree;
        }

        public void ResolveCoordinates()
        {
            var root = pointsDict.Values.Any(point => point.Point.HasValue);

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
                //foreach (var VARIABLE in used)
                //{
                    
                //}
            }
        }
    }
}