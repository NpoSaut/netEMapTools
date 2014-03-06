using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using EMapNavigator.MapElements;
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
        private Color[] _sectionColors =
            new[]
            {
                Color.FromRgb(255, 255, 102),
                Color.FromRgb(255, 204, 102),
                Color.FromRgb(255, 204, 153),
                Color.FromRgb(255, 153, 153),
                Color.FromRgb(255, 102, 51),
                Color.FromRgb(255, 51, 51),
                Color.FromRgb(255, 102, 204),
                Color.FromRgb(153, 102, 255),
                Color.FromRgb(102, 153, 255),
                Color.FromRgb(102, 204, 204),
                Color.FromRgb(0, 255, 51),
            };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            GMap gMap;
            using (var mapStream = new FileStream(Path.Combine("maps", "Екатеринбург.gps"), FileMode.Open))
            {
                gMap = GMap.Load(mapStream);
            }

            foreach (var section in gMap.Sections)
            {
                var sectionBrush = new SolidColorBrush(_sectionColors[r.Next(_sectionColors.Length)]);
                for (int i = 0; i < section.Posts.Count; i++)
                {
                    var post = section.Posts[i];
                    map.AddElement(new KilometerPostMapElement(post) { SectionBrush = sectionBrush});
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
