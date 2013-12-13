using System;
using Common.Logging;

namespace Cave.Core
{
    public abstract class CaveReaderBase
    {
        public abstract CaveGraph ReadCave(string file);

        protected bool ParseSpatialData(string azimuth, string vangle, string length, out double azim, out double vangl, out double len)
        {
            azim = vangl = 0;

            if (double.TryParse(length, out len) && double.TryParse(vangle, out vangl))
            {
                if (double.TryParse(azimuth, out azim))
                {
                    azim -= 180;
                }
                else if (90 == Math.Abs(vangl))
                {
                    azim = 0; //default azimuth when it is not significant
                }
                else
                {
                    LogManager.GetCurrentClassLogger().ErrorFormat(
                        "Cannot parse input data. Azimith is not correct. Azimuth={0}, Vangle={1}", azimuth, vangl);
                    throw new ArgumentException("Cannot parse input data");
                }

                return true;
            }

            return false;
        }

        
    }
}