using System;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Cave.Core;
using Common.Logging;

namespace Cave.Core
{
    public interface IGeometry
    {
        Geometry3D Model { get; }
    }
}
