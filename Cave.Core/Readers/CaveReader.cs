using System;
using System.IO;

namespace Cave.Core
{
    public static class CaveReader
    {
        public static CaveGraph ReadCave(string file)
        {
            var reader = GetCaveReader(file);
            return reader.ReadCave(file);
        }

        private static CaveReaderBase GetCaveReader(string file)
        {
            var ext = Path.GetExtension(file);
            switch (ext)
            {
                case ".csv":
                    return new CsvCaveReader();
                case ".mth":
                    return new MthCaveReader();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}