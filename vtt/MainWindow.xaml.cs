using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vtt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private SolidColorBrush gridBrush;
        private double gridOpacity = 0.5;
        private int gridLineSpacing = 20;
        private bool isDynamic = false;

        private bool scaleToGrid = true;
        private int gridScaleInches = 1;
        private double screenPixelWidth = 3840;
        private double screenPixelHeight = 2160;
        private double screenPhysicalWidth = 13.6;
        private double screenPhysicalHeight = 7.6;
        private double screenDpiScaling = 2.5;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridBrush = new SolidColorBrush(Colors.Cyan);
            gridBrush.Opacity = gridOpacity;

            if(isDynamic)
            {
                mapCanvas.Visibility = Visibility.Hidden;
                ShowVideo();
            }
            else
            {
                vttMediaViewer.Visibility = Visibility.Hidden;
                ShowMap();
            }

        }

        private void ShowMap()
        {
            string path = @"C:\Users\Jason\Documents\RPG\Maps\temple.jpg";
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
            ib.Stretch = Stretch.Uniform;
            mapCanvas.Background = ib;

            if(scaleToGrid)
            {
                ScaleMap();
            }
            DrawGrid();
            
        }

        private void ScaleMap()
        {
            // display grid calibration tool - establishes scaling of map image (pixels to distance on map)


            // sample code...just makes a 1" grid
            double hscale = screenPixelWidth / screenPhysicalWidth;
            double vscale = screenPixelHeight / screenPhysicalHeight;

            gridLineSpacing = (int)(((hscale + vscale) / 2)/screenDpiScaling);
        }

        private void ShowVideo()
        {
            vttMediaViewer.Source = new Uri(@"C:\Users\Jason\Documents\Unity\vtt\Assets\MapImages\Docks_gridless.m4v");
            vttMediaViewer.LoadedBehavior = MediaState.Manual;

            if(scaleToGrid)
            {
                ScaleMap();
            }
            DrawGrid();

            vttMediaViewer.Play();
        }

        private void DrawGrid()
        {
            gridCanvas.Children.Clear();

            double width = gridCanvas.ActualWidth;
            double height = gridCanvas.ActualHeight;

            for(int i=0;i<height; i+=gridLineSpacing)
            {
                Line line = new Line();
                line.Stroke = gridBrush;

                line.X1 = 0;
                line.X2 = width;
                line.Y1 = i;
                line.Y2 = i;

                line.StrokeThickness = 2;
                gridCanvas.Children.Add(line);
            }

            for (int i = 0; i < width; i += gridLineSpacing)
            {
                Line line = new Line();
                line.Stroke = gridBrush;
                line.X1 = i;
                line.X2 = i;
                line.Y1 = 0;
                line.Y2 = height;

                line.StrokeThickness = 1;
                gridCanvas.Children.Add(line);
            }

        }

        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGrid();
        }
    }
}
