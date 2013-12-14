using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Media.Media3D;
using Cave.Core;
using HelixToolkit.Wpf;
using System.Collections.Generic;

namespace Cave.Viewer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Model3D model;
        public Model3D Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged("Model"); }
        }

        private CaveViewType viewType = CaveViewType.Lines;
        public CaveViewType ViewType
        {
            get { return viewType; }
            set { viewType = value; CreateModel(); }
        }

        private List<CavePoint> _cavePoints;
        public List<CavePoint> CavePoints
        {
            get { return _cavePoints; }
            set { _cavePoints = value; RaisePropertyChanged("CavePoints"); }
        }

        private CaveGraph caveGraph;

        public MainViewModel(string file)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            caveGraph = CaveGraph.ReadCave(file);
            CreateModel();
        }

        private void CreateModel()
        {
            var m = new Model3DGroup();
            var caveModel = caveGraph.Render(ViewType);
            foreach (var geometry in caveModel.Geometry)
            {
                m.Children.Add(new GeometryModel3D(geometry.Model, Materials.Brown){BackMaterial = Materials.Blue});
            }
            Model = m;
            CavePoints = caveGraph.PointsDict.Values.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}