using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using EMapNavigator.Settings.Interfaces;
using Geographics;
using MapViewer.Mapping;
using MapVisualization;
using MapVisualization.Elements;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    public class MapViewModel : ReactiveObject, IMappingService
    {
        private readonly ObservableCollection<MapElement> _elements = new ObservableCollection<MapElement>();
        private readonly ReactiveCommand<object> _mapClickedCommand;
        private EarthPoint _mapCenter;

        private int _zoomLevel;

        public MapViewModel(IMapPositionSettings PositionSettings)
        {
            MapCenter = PositionSettings.MapCenter;
            ZoomLevel = PositionSettings.ZoomLevel;

            this.WhenAnyValue(x => x.MapCenter).Subscribe(c => PositionSettings.MapCenter = c);
            this.WhenAnyValue(x => x.ZoomLevel).Subscribe(z => PositionSettings.ZoomLevel = z);

            _mapClickedCommand = ReactiveCommand.Create();

            Clicks = _mapClickedCommand.OfType<MapMouseActionEventArgs>();
        }

        public int ZoomLevel
        {
            get { return _zoomLevel; }
            set { this.RaiseAndSetIfChanged(ref _zoomLevel, value); }
        }

        public EarthPoint MapCenter
        {
            get { return _mapCenter; }
            private set { this.RaiseAndSetIfChanged(ref _mapCenter, value); }
        }

        public ICommand MapClickedCommand
        {
            get { return _mapClickedCommand; }
        }

        public ObservableCollection<MapElement> Elements
        {
            get { return _elements; }
        }

        public void Display(MapElement Element) { Elements.Add(Element); }
        public void Remove(MapElement Element) { Elements.Remove(Element); }
        public IObservable<MapMouseActionEventArgs> Clicks { get; private set; }
        public void Navigate(EarthPoint ToPoint) { MapCenter = ToPoint; }
    }
}
