using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;

namespace Cave.Core
{
    public class CsvCaveReader : CaveReaderBase
    {
        public override CaveGraph ReadCave(string csvFilePath)
        {
            var lines = File.ReadAllLines(csvFilePath);
            var tree = new CaveGraph();

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                CavePoint p1 = tree.GetPoint(parts[0], tree);
                p1.Distances = parts.Where((p, ind) => ind > 4 && ind <= 8).Select(p =>
                {
                    double d;
                    return double.TryParse(p, out d) ? d : -1;
                }).ToArray();

                double x, y, z;
                if (double.TryParse(parts[9], out x) && double.TryParse(parts[10], out y) && double.TryParse(parts[11], out z))
                {
                    p1.Point = new Point3D(x, y, z);
                }

                CavePoint p2 = tree.GetPoint(parts[1], tree);

                double len, azimuth, vangel;
                if (ParseSpatialData(parts[3], parts[2], parts[4], out azimuth, out vangel, out len))
                {
                    tree.AddEdge(p1, new CaveEdge(p1, p2, len, azimuth, vangel));
                }
                else
                {
                    LogManager.GetCurrentClassLogger().ErrorFormat("Cannot parse input data");
                    throw new ArgumentException("Cannot parse input data");
                }
            }

            LogManager.GetCurrentClassLogger().InfoFormat("Input cave {0} parsed successfully", csvFilePath);
            return tree;
        }
    }
}