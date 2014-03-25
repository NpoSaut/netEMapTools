using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Communications;
using Communications.Appi;
using Communications.Appi.Winusb;
using EMapNavigator.Emulation;
using EMapNavigator.MapElements;
using Geographics;
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

        public IPathRider PathRider { get; set; }
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
                    this.Title = SelectingTrack.Length.ToString();
                    break;

                case MouseAction.RightClick:
                    SelectingTrack = null;
                    Map.RemoveElement(_previousMapTrackElement);
                    break;
            }
        }

        private CanWheel _wheel;
        private void RideButton_OnClick(object Sender, RoutedEventArgs e)
        {
            PathRider = new TrackRider(SelectingTrack);
            step = 0;
            point = new MapMarkerElement(new EarthPoint());
            Map.AddElement(point);

            var dev = WinusbAppiDev.GetDevices().First().OpenDevice();
            dev.CanPorts[AppiLine.Can1].BaudRate = BaudRates.CBR_100K;
            dev.CanPorts[AppiLine.Can2].BaudRate = BaudRates.CBR_100K;
            _wheel = new CanWheel(dev.CanPorts[AppiLine.Can1]);
            _wheel.Speed = 15;
            _wheel.MilageChanged += WheelOnMilageChanged;
        }

        private void WheelOnMilageChanged(object Sender, EventArgs Args)
        {
            var epoint = PathRider.PointAt(_wheel.Milage);
            Debug.Print("POINT: {0}  | DIST: {1}", epoint, _wheel.Milage);
            Dispatcher.BeginInvoke((Action<EarthPoint>)(p => point.Position = p), epoint);
        }

        private double step;
        private MapMarkerElement point;
        private void MakeAStep()
        {
            step += 700;
            var epoint = PathRider.PointAt(step);
            Debug.Print("DIST: {0}", point.Position.DistanceTo(epoint));
            point.Position = epoint;
        }
    }
}
