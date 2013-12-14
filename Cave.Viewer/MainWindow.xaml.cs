using System.Windows;
using Cave.Core;
using HelixToolkit.Wpf;
using Microsoft.Win32;

namespace Cave.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(".\\Examples\\cave0.csv");
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).ViewType = CaveViewType.Lines;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).ViewType = CaveViewType.Tubes;
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Image (*.jpg)|*.jpg|X3D (*.x3d)|*.x3d|Kerkythea (*.xml)|*.xml|Collada (*.dae)|*.dae|XAML (*.xaml)|*.xaml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                var file = saveFileDialog1.FileName;
                MainViewport3D.Export(file);
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "MTH (*.mth)|*.mth|CSV (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                var file = openFileDialog1.FileName;
                DataContext = new MainViewModel(file);
                MainViewport3D.ResetCamera();
            }
        }

        private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainViewport3D.ResetCamera();
        }

        private void ListBox_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedPoint = (CavePoint)PointsListBox.SelectedItem;
            var ind = PointsListBox.SelectedIndex;
            if (selectedPoint.Point.HasValue)
            {
                MainViewport3D.Camera.Position = selectedPoint.Point.Value;
                
                var nextPoint = (CavePoint) PointsListBox.Items[PointsListBox.Items.Count-1>ind ? ind + 1 : ind - 1];
                if (nextPoint.Point.HasValue)
                {
                    MainViewport3D.Camera.LookDirection = nextPoint.Point.Value - selectedPoint.Point.Value;
                }
            }
        }

        private void InspectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainViewport3D.CameraMode = CameraMode.Inspect;
        }

        private void WalkArroundMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainViewport3D.CameraMode = CameraMode.WalkAround;
        }
    }
}
