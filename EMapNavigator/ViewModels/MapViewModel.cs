using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using EMapNavigator.Settings.Interfaces;
using Geographics;
using MapViewer.Mapping;
using MapViewer.Settings.Interfaces;
using MapVisualization;
using MapVisualization.Elements;
using MapVisualization.TileLoaders;
using MapVisualization.TileLoaders.TilePathProvider;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    public class MapViewModel : ReactiveObject, IMappingService
    {
        private readonly IMapAppearanceSettings _appearanceSettings;
        private readonly ObservableCollection<MapElement> _elements = new ObservableCollection<MapElement>();
        private readonly ReactiveCommand<object> _mapClickedCommand;
        private readonly ObservableAsPropertyHelper<ITileLoader> _tileLoader;
        private EarthPoint _mapCenter;

        private int _zoomLevel;

        public MapViewModel(IMapPositionSettings PositionSettings, IMapAppearanceSettings AppearanceSettings)
        {
            _appearanceSettings = AppearanceSettings;
            MapCenter = PositionSettings.MapCenter;
            ZoomLevel = PositionSettings.ZoomLevel;

            this.WhenAnyValue(x => x.MapCenter).Subscribe(c => PositionSettings.MapCenter = c);
            this.WhenAnyValue(x => x.ZoomLevel).Subscribe(z => PositionSettings.ZoomLevel = z);

            this.WhenAnyValue(x => x._appearanceSettings.HighResolutionTiles)
                .StartWith(_appearanceSettings.HighResolutionTiles)
                .Select(ChooseTileLoader)
                .ToProperty(this, x => x.TileLoader, out _tileLoader);

            _mapClickedCommand = ReactiveCommand.Create();

            Clicks = _mapClickedCommand.OfType<MapMouseActionEventArgs>();
        }

        public ITileLoader TileLoader
        {
            get { return _tileLoader.Value; }
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

        private ITileLoader ChooseTileLoader(bool UseHighResolutionTiles)
        {
            return UseHighResolutionTiles
                       ? new WebTileLoader(OsmTilePathProviders.Retina)
                       : new WebTileLoader(OsmTilePathProviders.Default);
        }
    }
}
