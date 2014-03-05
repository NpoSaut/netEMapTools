using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using MapVisualization;

namespace EMapNavigator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GMap gMap;
            using (var mapStream = new FileStream("Екатеринбург.gps", FileMode.Open))
            {
                gMap = GMap.Load(mapStream);
            }

            foreach (var section in gMap.Sections)
            {
                for (int i = 0; i < section.Posts.Count; i++)
                {
                    var post = section.Posts[i];
                    map.AddElement(new KilometerPostMapElement(post));
                    if (i + 1 < section.Posts.Count)
                    {
                        var nextPost = section.Posts[i + 1];
                        foreach (var gObject in post.Tracks.First().Objects)
                        {
                        }
                    }
                }
            }
        }

        private readonly List<EarthPoint> _trackPoints = new List<EarthPoint>();
        private MapTrackElement _previousMapTrackElement;

        private void Map_OnGeographicMouseClick(object Sender, GeographicEventArgs E)
        {
            _trackPoints.Add(E.Point);
            if (_previousMapTrackElement != null) map.RemoveElement(_previousMapTrackElement);
            _previousMapTrackElement = new MapTrackElement(_trackPoints, new Pen(Brushes.BlueViolet, 2));
            map.AddElement(_previousMapTrackElement);
        }
    }
}
