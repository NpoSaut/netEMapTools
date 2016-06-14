using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BlokFrames;
using BlokMap.MapElements.MapObjectElements;
using EMapNavigator.Emition;
using EMapNavigator.Emulation;
using EMapNavigator.MapElements;
using EMapNavigator.ViewModels;
using Geographics;
using GMapElements;
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
    }
}
