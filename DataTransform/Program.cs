using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransform
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>();
            foreach (var line in File.ReadAllLines("cave0.txt"))
            {
                var pp = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                string l = "";
                foreach (var p in pp)
                {
                    l+=","+p;
                }
                l += ",,,";
                lines.Add(l.TrimStart(','));
            }

            File.WriteAllLines("cave0.csv", lines);
        }
    }
}
