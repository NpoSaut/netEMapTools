using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using EMapNavigator.MapElements;
using GMapElements;
using MapVisualization;
using Tracking;

namespace EMapNavigator
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        #region Цвета для сегментов

        private readonly Color[] _sectionColors =
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
            Color.FromRgb(0, 255, 51)
        };

        #endregion

        public GpsTrack SelectingTrack { get; private set; }
        private MapTrackElement _previousMapTrackElement;
        public MainWindow() { InitializeComponent(); }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            GMap gMap;
            using (var mapStream = new FileStream(Path.Combine("maps", "Екатеринбург.gps"), FileMode.Open))
            {
                gMap = GMap.Load(mapStream);
            }

            foreach (GSection section in gMap.Sections)
            {
                var sectionBrush = new SolidColorBrush(_sectionColors[r.Next(_sectionColors.Length)]);
                for (int i = 0; i < section.Posts.Count; i++)
                {
                    GPost post = section.Posts[i];
                    Map.AddElement(new KilometerPostMapElement(post) { SectionBrush = sectionBrush });
                    if (i + 1 < section.Posts.Count)
                    {
                        GPost nextPost = section.Posts[i + 1];
                        foreach (GObject gObject in post.Tracks.First().Objects) { }
                    }
                }
            }
        }

        private void Map_OnClick(object Sender, MapMouseActionEventArgs E)
        {
            switch (E.Action)
            {
                case MouseAction.LeftClick:
                    if (SelectingTrack == null) SelectingTrack = new GpsTrack();
                    SelectingTrack.TrackPoints.Add(E.Point);
                    if (_previousMapTrackElement != null) Map.RemoveElement(_previousMapTrackElement);
                    _previousMapTrackElement = new MapTrackElement(SelectingTrack.TrackPoints,
                                                                   new Pen(Brushes.BlueViolet, 2));
                    Map.AddElement(_previousMapTrackElement);
                    break;

                case MouseAction.RightClick:
                    SelectingTrack = null;
                    Map.RemoveElement(_previousMapTrackElement);
                    break;
            }
        }
    }
}
