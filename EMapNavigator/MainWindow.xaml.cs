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
using System.Xml.Linq;
using BlokFrames;
using BlokMap.MapElements.MapObjectElements;
using EMapNavigator.Emition;
using EMapNavigator.Emulation;
using EMapNavigator.MapElements;
using EMapNavigator.ViewModels;
using Geographics;
using GMapElements;
using MapVisualization;
using MapVisualization.Elements;
using Tracking;
using Tracking.MapElements;

namespace EMapNavigator
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        private static readonly int[] DebugDescriptors = BlokFrame.GetDescriptors<MmAltLongFrame>().Values.ToArray();
        private readonly object _currentPointLocker = new object();

        private EarthPoint _currentPoint;
        private MapMarkerElement _displayPoint;
        private IEmitter _emitter;
        private GMap _gMap;
        private IWheel _wheel;

        public MainWindow()
        {
            MapElements = new ObservableCollection<MapElement>();
            InitializeComponent();
        }

        public IPathRider PathRider { get; set; }
        public GpsTrack SelectingTrack { get; private set; }

        public ObservableCollection<MapElement> MapElements { get; private set; }
        public WheelViewModel Wheel { get; private set; }

        private void Map_OnClick(object Sender, MapMouseActionEventArgs E)
        {
            switch (E.Action)
            {
                case MouseAction.LeftClick:
                    if (SelectingTrack == null)
                        SelectingTrack = new GpsTrack();
                    SelectingTrack.TrackPoints.Add(E.Point);
                    RefreshTrack();
                    break;

                case MouseAction.RightClick:
                    SelectingTrack = null;
                    RefreshTrack();
                    break;
            }
        }

        private void RefreshTrack()
        {
            //if (_previousMapTrackElement != null)
            //    MapElements.Remove(_previousMapTrackElement);
            //if (SelectingTrack != null)
            //{
            //    new MapTrackElement(SelectingTrack.TrackPoints,
            //                        new Pen(Brushes.BlueViolet, 2));
            //    MapElements.Add(_previousMapTrackElement);
            //    Title = string.Format("Длина трека: {0:F1} м", SelectingTrack.Length);
            //}
        }

        private void RideButton_OnClick(object Sender, RoutedEventArgs e)
        {
            PathRider = new TrackRider(SelectingTrack);
            if (_displayPoint != null)
                MapElements.Remove(_displayPoint);
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
            //WheelView.DataContext = Wheel;

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
            _emitter.EmitPosition(localCurrentPoint, _wheel.Speed);
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

            //PrintObjects(_gMap, 1);
        }

        private void SaveTrackButton_Click(object Sender, RoutedEventArgs e)
        {
            new XDocument(
                new XElement("Track",
                             SelectingTrack.TrackPoints.Select(p =>
                                                               new XElement("Point",
                                                                            new XAttribute("Latitude", p.Latitude.Value),
                                                                            new XAttribute("Longitude", p.Longitude.Value)))))
                .Save("track.xml");
        }

        private void LoadTrackButton_Click(object Sender, RoutedEventArgs e)
        {
            if (!File.Exists("track.xml"))
                return;
            XDocument doc = XDocument.Load("track.xml");
            SelectingTrack = new GpsTrack(
                doc.Root.Elements("Point")
                   .Select(XPoint => new EarthPoint((double)XPoint.Attribute("Latitude"),
                                                    (double)XPoint.Attribute("Longitude")))
                   .ToList());
            RefreshTrack();
        }
    }
}
