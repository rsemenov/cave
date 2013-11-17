using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using Cave.Core;

namespace Cave.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private Model3D model;
        public Model3D Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged("Model"); }
        }

        public MainViewModel()
        {
            CreateModel();
        }

        private void CreateModel()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var m = new Model3DGroup();

            var graph = CaveGraph.ReadCave(".\\Examples\\cave1.csv");

            var caveModel = graph.Render(CaveViewType.Tubes);

            foreach (var geometry in caveModel.Geometry)
            {
                m.Children.Add(new GeometryModel3D(geometry.Model, Materials.Brown){BackMaterial = Materials.Blue});
            }

            /*var tm = new MeshBuilder(false, false);

            List<Point3D> points = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 0, 4) };
            var diameters = new double[] {6, 2};

            tm.AddTube(points, new double[]{0,0}, diameters, 20, false);

            var mesh = tm.ToMesh();
            m.Children.Add(new GeometryModel3D(tm.ToMesh(), Materials.Red) { BackMaterial = Materials.Blue });
             */
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

    public class PanelModelBuilder
    {
        public List<Panel> Panels { get; set; }

        public class Panel
        {
            public Point3D[] Points { get; set; }
            public int TriangleIndex { get; set; }
        }

        public List<int> TriangleIndexToPanelIndex { get; set; }

        public PanelModelBuilder()
        {
            Panels = new List<Panel>();
        }

        public void AddPanel(params Point3D[] points)
        {
            Panels.Add(new Panel { Points = points });
        }

        public void AddPanel(params double[] coords)
        {
            Point3D[] points = new Point3D[coords.Length / 3];
            for (int i = 0; i < coords.Length / 3; i++) points[i] = new Point3D(coords[i * 3], coords[i * 3 + 1], coords[i * 3 + 2]);
            AddPanel(points);
        }

        public Model3D ToModel3D()
        {
            var m = new Model3DGroup();

            TriangleIndexToPanelIndex = new List<int>();

            // Add the triangles
            var tm = new MeshBuilder(false, false);
            int panelIndex = 0;
            foreach (var p in Panels)
            {
                p.TriangleIndex = tm.Positions.Count;
                tm.AddTriangleFan(p.Points);
                for (int i = 0; i < p.Points.Length - 2; i++) TriangleIndexToPanelIndex.Add(panelIndex);
                panelIndex++;
            }
            var panelsGeometry = tm.ToMesh();
            m.Children.Add(new GeometryModel3D(panelsGeometry, Materials.Red) { BackMaterial = Materials.Blue });

            // Add the nodes
            var gm = new MeshBuilder();
            foreach (var p in panelsGeometry.Positions)
            {
                gm.AddSphere(p, 0.05);
            }
            m.Children.Add(new GeometryModel3D(gm.ToMesh(), Materials.Gold));

            // Add the edges
            var em = new MeshBuilder();
            foreach (var p in Panels)
            {
                for (int i = 0; i < p.Points.Length; i += 1)
                {
                    em.AddCylinder(p.Points[i], p.Points[(i + 1) % p.Points.Length], 0.05, 10);
                }
            }
            m.Children.Add(new GeometryModel3D(em.ToMesh(), Materials.Gray));

            return m;
        }
    }
}
