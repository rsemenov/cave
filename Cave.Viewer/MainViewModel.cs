using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Media.Media3D;
using Cave.Core;
using HelixToolkit.Wpf;

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

        private CaveGraph caveGraph;

        public MainViewModel()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            caveGraph = CaveGraph.ReadCave(".\\Examples\\cave0.csv");
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