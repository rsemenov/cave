using System.Windows;
using Cave.Core;
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
            DataContext = new MainViewModel();
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
    }
}
