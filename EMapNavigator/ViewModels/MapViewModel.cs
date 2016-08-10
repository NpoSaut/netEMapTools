using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
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

        public MapViewModel()
        {
            //MapCenter = new EarthPoint(new Degree(56.8779), new Degree(60.5905));
            MapCenter = new EarthPoint(new Degree(55.729959), new Degree(37.540420));
            ZoomLevel = 14;

            _mapClickedCommand = ReactiveCommand.Create();

            Clicks = _mapClickedCommand.OfType<MapMouseActionEventArgs>();
        }

        public int ZoomLevel { get; set; }

        public EarthPoint MapCenter { get; private set; }

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
    }
}
