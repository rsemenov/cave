using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;

namespace Cave.Core
{
    public class MthCaveReader : CaveReaderBase
    {
        public override CaveGraph ReadCave(string file)
        {
            var cave = new CaveGraph();
            var lines = File.ReadAllLines(file);
            
            int i;

            for (i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var templ = line.Replace(" ", "").ToLowerInvariant();

                if (string.IsNullOrEmpty(templ))
                    continue;

                if (templ == "datanormalfromtobackcompassclinolength")
                {
                    continue;
                }
                if (templ == "datadimensionsstationleftrightupdown")
                {
                    break;
                }

                var parts = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

                CavePoint p1 = cave.GetPoint(parts[0], cave);
                CavePoint p2 = cave.GetPoint(parts[1], cave);

                double len, azimuth, vangel;
                if (ParseSpatialData(parts[2], parts[3], parts[4], out azimuth, out vangel, out len))
                {
                    cave.AddEdge(p1, new CaveEdge(p1, p2, len, azimuth, 90 - vangel));
                }
                else
                {
                    LogManager.GetCurrentClassLogger().ErrorFormat("Cannot parse input data. Azimuth={0}, Vangle={1}, Lenght={2}", parts[2], parts[3], parts[4]);
                    throw new ArgumentException("Cannot parse input data");
                }
            }

            for (; i < lines.Length; i++)
            {
                var line = lines[i];
                var templ = line.Replace(" ", "").ToLowerInvariant();

                if (string.IsNullOrEmpty(templ))
                    continue;
                
                var parts = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                CavePoint p1 = cave.GetPoint(parts[0], cave);

                p1.Distances = parts.Where((p, ind) => ind > 1 && ind <= 4).Select(p =>
                {
                    double d;
                    return double.TryParse(p, out d) ? d : -1;
                }).ToArray();
            }

            var firstPoint = cave.PointsDict.FirstOrDefault().Value;
            firstPoint.Point = new Point3D(0,0,0);

            return cave;
        }

        
    }
}