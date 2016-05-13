using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BlokFrames;
using EMapNavigator.Emition;
using EMapNavigator.Emulation;
using EMapNavigator.MapElements;
using EMapNavigator.MapElements.MapObjectElements;
using EMapNavigator.ViewModels;
using Geographics;
using GMapElements;
using MapVisualization;
using MapVisualization.Elements;
using Microsoft.Win32;
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

        private static readonly int[] DebugDescriptors = BlokFrame.GetDescriptors<MmAltLongFrame>().Values.ToArray();
        private readonly object _currentPointLocker = new object();

        private readonly IList<EarthPoint> _trackPoints = new[]
                                                          {
                                                              new EarthPoint(new Radian(0.75802315), new Radian(0.69697016)),
                                                              new EarthPoint(new Radian(0.75802565), new Radian(0.69696740)),
                                                              new EarthPoint(new Radian(0.75802810), new Radian(0.69696460)),
                                                              new EarthPoint(new Radian(0.75803278), new Radian(0.69695887)),
                                                              new EarthPoint(new Radian(0.75803499), new Radian(0.69695587)),
                                                              new EarthPoint(new Radian(0.75803499), new Radian(0.69695587)),
                                                              new EarthPoint(new Radian(0.75803709), new Radian(0.69695277)),
                                                              new EarthPoint(new Radian(0.75803910), new Radian(0.69694960)),
                                                              new EarthPoint(new Radian(0.75804104), new Radian(0.69694634)),
                                                              new EarthPoint(new Radian(0.75804287), new Radian(0.69694299)),
                                                              new EarthPoint(new Radian(0.75804464), new Radian(0.69693965)),
                                                              new EarthPoint(new Radian(0.75804633), new Radian(0.69693627)),
                                                              new EarthPoint(new Radian(0.75804799), new Radian(0.69693292)),
                                                              new EarthPoint(new Radian(0.75804965), new Radian(0.69692958)),
                                                              new EarthPoint(new Radian(0.75805128), new Radian(0.69692632)),
                                                              new EarthPoint(new Radian(0.75805288), new Radian(0.69692307)),
                                                              new EarthPoint(new Radian(0.75805448), new Radian(0.69691984)),
                                                              new EarthPoint(new Radian(0.75805607), new Radian(0.69691667)),
                                                              new EarthPoint(new Radian(0.75805764), new Radian(0.69691352)),
                                                              new EarthPoint(new Radian(0.75805921), new Radian(0.69691041)),
                                                              new EarthPoint(new Radian(0.75806079), new Radian(0.69690733)),
                                                              new EarthPoint(new Radian(0.75806230), new Radian(0.69690427)),
                                                              new EarthPoint(new Radian(0.75806381), new Radian(0.69690127)),
                                                              new EarthPoint(new Radian(0.75806532), new Radian(0.69689832)),
                                                              new EarthPoint(new Radian(0.75806678), new Radian(0.69689538)),
                                                              new EarthPoint(new Radian(0.75806823), new Radian(0.69689247)),
                                                              new EarthPoint(new Radian(0.75806965), new Radian(0.69688956)),
                                                              new EarthPoint(new Radian(0.75807108), new Radian(0.69688665)),
                                                              new EarthPoint(new Radian(0.75807390), new Radian(0.69688080)),
                                                              new EarthPoint(new Radian(0.75807530), new Radian(0.69687786)),
                                                              new EarthPoint(new Radian(0.75807806), new Radian(0.69687202)),
                                                              new EarthPoint(new Radian(0.75808077), new Radian(0.69686617)),
                                                              new EarthPoint(new Radian(0.75808211), new Radian(0.69686326)),
                                                              new EarthPoint(new Radian(0.75808339), new Radian(0.69686035)),
                                                              new EarthPoint(new Radian(0.75808467), new Radian(0.69685747)),
                                                              new EarthPoint(new Radian(0.75808580), new Radian(0.69685462)),
                                                              new EarthPoint(new Radian(0.75808699), new Radian(0.69685174)),
                                                              new EarthPoint(new Radian(0.75808822), new Radian(0.69684884)),
                                                              new EarthPoint(new Radian(0.75808944), new Radian(0.69684587)),
                                                              new EarthPoint(new Radian(0.75809066), new Radian(0.69684293)),
                                                              new EarthPoint(new Radian(0.75809191), new Radian(0.69683996)),
                                                              new EarthPoint(new Radian(0.75809316), new Radian(0.69683699)),
                                                              new EarthPoint(new Radian(0.75809444), new Radian(0.69683405)),
                                                              new EarthPoint(new Radian(0.75809577), new Radian(0.69683117)),
                                                              new EarthPoint(new Radian(0.75809714), new Radian(0.69682829)),
                                                              new EarthPoint(new Radian(0.75809848), new Radian(0.69682544)),
                                                              new EarthPoint(new Radian(0.75809985), new Radian(0.69682260)),
                                                              new EarthPoint(new Radian(0.75810121), new Radian(0.69681975))
                                                          };

        private EarthPoint _currentPoint;
        private MapMarkerElement _displayPoint;
        private IEmitter _emitter;
        private GMap _gMap;
        private MapTrackElement _previousMapTrackElement;
        private IWheel _wheel;

        public MainWindow()
        {
            MapElements = new ObservableCollection<MapElement>();
            InitializeComponent();
            TrackSelector.ItemsSource = Enumerable.Range(1, 29);
            TrackSelector.SelectedItem = 2;
            //Map.CentralPoint = new EarthPoint(new Radian(0.75806079), new Radian(0.69690733));
            Map.CentralPoint = new EarthPoint(new Degree(56.8779), new Degree(60.5905));
            Map.ZoomLevel = 14;
            Map.ElementsSource = MapElements;
            MapElements.Add(new MapTrackElement(_trackPoints, new Pen(Brushes.MediumVioletRed, 2)));
        }

        public IPathRider PathRider { get; set; }
        public GpsTrack SelectingTrack { get; private set; }

        public ObservableCollection<MapElement> MapElements { get; private set; }
        public WheelViewModel Wheel { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                using (var mapStream = new FileStream(dlg.FileName, FileMode.Open))
                {
                    _gMap = GMap.Load(mapStream);
                }

                PrintPosts(_gMap);
                PrintObjects(_gMap, (int)TrackSelector.SelectedItem);
            }
        }

        private void PrintPosts(GMap gMap)
        {
            var r = new Random();
            foreach (GSection section in gMap.Sections)
            {
                var sectionBrush = new SolidColorBrush(_sectionColors[r.Next(_sectionColors.Length)]);
                foreach (GPost post in section.Posts)
                    MapElements.Add(new KilometerPostMapElement(post) { SectionBrush = sectionBrush });
            }
        }

        private void PrintObjects(GMap gMap, int trackNumber)
        {
            foreach (GSection section in gMap.Sections)
            {
                for (int i = 0; i < section.Posts.Count; i++)
                {
                    GPost post = section.Posts[i];
                    if (i + 1 < section.Posts.Count)
                    {
                        GPost nextPost = section.Posts[i + 1];
                        double dist = post.Point.DistanceTo(nextPost.Point);

                        GTrack track = post.Tracks.FirstOrDefault(t => t.Number == trackNumber);

                        if (track != null)
                        {
                            foreach (GObject gObject in track.Objects)
                            {
                                double objectRatio = (gObject.Ordinate - post.Ordinate) / dist;
                                EarthPoint objectPosition = EarthPoint.MiddlePoint(post.Point, nextPost.Point, objectRatio);
                                MapElement objectElement;
                                switch (gObject.Type)
                                {
                                    case GObjectType.TrafficLight:
                                        objectElement = new MapTrafficLightElement(objectPosition, gObject);
                                        break;
                                    case GObjectType.Platform:
                                    case GObjectType.Station:
                                        objectElement = new MapPlatformElement(objectPosition, gObject);
                                        break;
                                    default:
                                        objectElement = new MapUnknownObjectElement(objectPosition, gObject);
                                        break;
                                }
                                MapElements.Add(objectElement);
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
                    if (_previousMapTrackElement != null) MapElements.Remove(_previousMapTrackElement);
                    _previousMapTrackElement = new MapTrackElement(SelectingTrack.TrackPoints,
                                                                   new Pen(Brushes.BlueViolet, 2));
                    MapElements.Add(_previousMapTrackElement);
                    Title = string.Format("Длина трека: {0:F1} м", SelectingTrack.Length);
                    break;

                case MouseAction.RightClick:
                    SelectingTrack = null;
                    MapElements.Remove(_previousMapTrackElement);
                    break;
            }
        }

        private void RideButton_OnClick(object Sender, RoutedEventArgs e)
        {
            PathRider = new TrackRider(SelectingTrack);
            _displayPoint = new MapMarkerElement(new EarthPoint());
            MapElements.Add(_displayPoint);

            //var appiDeviceFactory = new SingletonAppiDeviceFactory();
            //var wheelFactory = new CanWheelFactory(appiDeviceFactory);
            //_emitter = new CanEmitter(appiDeviceFactory.GetDevice());

            var wheelFactory = new VirtualWheelFactory();
            _emitter = new LanEmitter();

            _wheel = wheelFactory.GetWheel();
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
            Console.WriteLine("moving on {0}", localCurrentPoint);
            _emitter.EmitPosition(localCurrentPoint);
        }

        private void WheelOnMilageChanged(object Sender, EventArgs Args)
        {
            lock (_currentPointLocker)
            {
                _currentPoint = PathRider.PointAt(_wheel.Milage);
            }
            Debug.Print("POINT: {0}  | DIST: {1}", _currentPoint, _wheel.Milage);
            Dispatcher.BeginInvoke((Action<EarthPoint>)(p => _displayPoint.Position = p), _currentPoint);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { Wheel.Speed = e.NewValue; }

        private void TrackSelector_OnSelectionChanged(object Sender, SelectionChangedEventArgs E)
        {
            if (_gMap == null) return;

            List<MapObjectElement> elementsToRemove = MapElements.OfType<MapObjectElement>().ToList();
            foreach (MapObjectElement element in elementsToRemove)
                MapElements.Remove(element);

            PrintObjects(_gMap, (int)TrackSelector.SelectedItem);
        }
    }
}
