using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BlokFrames;
using Communications;
using Communications.Appi;
using Communications.Appi.Winusb;
using Communications.Can;
using EMapNavigator.Emulation;
using EMapNavigator.MapElements;
using EMapNavigator.MapElements.MapObjectElements;
using EMapNavigator.ViewModels;
using Geographics;
using GMapElements;
using MapVisualization;
using MapVisualization.Elements;
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

        public MainWindow()
        {
            InitializeComponent();
            Map.CentralPoint = new EarthPoint(57.1268, 35.4619);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            GMap gMap;
            using (var mapStream = new FileStream(Path.Combine("maps", "msk-spb.gps"), FileMode.Open))
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
                        double dist = post.Point.DistanceTo(nextPost.Point);

                        var track = post.Tracks.FirstOrDefault(t => t.Number == 2);

                        if (track != null)
                        {
                            foreach (GObject gObject in track.Objects)
                            {
                                double objectRatio = (gObject.Ordinate - post.Ordinate) / dist;
                                EarthPoint objectPosition = EarthPoint.MiddlePoint(post.Point, nextPost.Point, objectRatio);
                                MapElement objectElement;
                                switch (gObject.Type)
                                {
                                    case GObjectType.TrafficLight: objectElement = new MapTrafficLightElement(objectPosition, gObject);
                                        break;
                                    case GObjectType.Platform:
                                    case GObjectType.Station:
                                        objectElement = new MapPlatformElement(objectPosition, gObject);
                                        break;
                                    default: objectElement = new MapUnknownObjectElement(objectPosition, gObject);
                                        break;
                                }
                                Map.AddElement(objectElement);
                            }
                        }
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
                    Title = string.Format("Длина трека: {0:F1} м", SelectingTrack.Length);
                    break;

                case MouseAction.RightClick:
                    SelectingTrack = null;
                    Map.RemoveElement(_previousMapTrackElement);
                    break;
            }
        }

        private AppiDev _appiDevice;

        public WheelViewModel Wheel { get; private set; }

        private CanWheel _wheel;
        private void RideButton_OnClick(object Sender, RoutedEventArgs e)
        {
            PathRider = new TrackRider(SelectingTrack);
            _displayPoint = new MapMarkerElement(new EarthPoint());
            Map.AddElement(_displayPoint);

            _appiDevice = WinusbAppiDev.GetDevices().First().OpenDevice();
            _appiDevice.CanPorts[AppiLine.Can1].BaudRate = BaudRates.CBR_100K;
            _appiDevice.CanPorts[AppiLine.Can2].BaudRate = BaudRates.CBR_100K;

            _appiDevice.CanPorts[AppiLine.Can1].Received += PortOnReceived;

            _wheel = new CanWheel(_appiDevice.CanPorts[AppiLine.Can1]);
            _wheel.MilageChanged += WheelOnMilageChanged;

            Wheel = new WheelViewModel(_wheel);
            WheelView.DataContext = Wheel;

            var emitLatLonTimer = new Timer(500);
            emitLatLonTimer.Elapsed += EmitLatLonTimerOnElapsed;
            emitLatLonTimer.Start();
        }

        private void EmitLatLonTimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            EarthPoint localCurrentPoint;
            lock (_currentPointLocker)
            {
                localCurrentPoint = _currentPoint;
            }
            var frame = new MmAltLongFrame(localCurrentPoint.Latitude, localCurrentPoint.Longitude);
            _appiDevice.CanPorts[AppiLine.Can1].Send(frame);
            
        }

        private void WheelOnMilageChanged(object Sender, EventArgs Args)
        {
            lock (_currentPointLocker)
            {
                _currentPoint = PathRider.PointAt(_wheel.Milage);
            }
            //Debug.Print("POINT: {0}  | DIST: {1}", _currentPoint, _wheel.Milage);
            Dispatcher.BeginInvoke((Action<EarthPoint>)(p => _displayPoint.Position = p), _currentPoint);
        }


        private EarthPoint _currentPoint;
        private readonly object _currentPointLocker = new object();

        private MapMarkerElement _displayPoint;

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Wheel.Speed = e.NewValue;
        }





        private static readonly int[] DebugDescriptors = BlokFrame.GetDescriptors<MmAltLongFrame>().Values.ToArray();

        private void PortOnReceived(object Sender, CanFramesReceiveEventArgs CanFramesReceiveEventArgs)
        {
            foreach (var frame in CanFramesReceiveEventArgs.Frames.Where(f => DebugDescriptors.Contains(f.Descriptor)))
            {
                Debug.Print(" ---> {0}", frame);
            }
        }
    }
}
