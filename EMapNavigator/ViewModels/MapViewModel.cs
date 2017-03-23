using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using EMapNavigator.MapElements;
using EMapNavigator.Settings.Interfaces;
using Geographics;
using MapViewer.Mapping;
using MapViewer.Settings.Interfaces;
using MapVisualization;
using MapVisualization.Annotations;
using MapVisualization.Elements;
using MapVisualization.TileLoaders;
using MapVisualization.TileLoaders.TilePathProvider;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    [UsedImplicitly]
    public class MapViewModel : ReactiveObject, IMappingService
    {
        private readonly IMapAppearanceSettings _appearanceSettings;
        private readonly ReactiveCommand<object> _mapClickedCommand;
        private readonly ObservableAsPropertyHelper<ITileLoader> _tileLoader;

        private bool _isPointerEnabled;
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

            this.WhenAnyValue(x => x.IsPointerEnabled)
                .DistinctUntilChanged()
                .Subscribe(PinMarker);

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

        public bool IsPointerEnabled
        {
            get { return _isPointerEnabled; }
            set { this.RaiseAndSetIfChanged(ref _isPointerEnabled, value); }
        }

        public ICommand MapClickedCommand
        {
            get { return _mapClickedCommand; }
        }

        public ObservableCollection<MapElement> Elements { get; } = new ObservableCollection<MapElement>();

        public EarthPoint MapCenter
        {
            get { return _mapCenter; }
            private set { this.RaiseAndSetIfChanged(ref _mapCenter, value); }
        }

        public void Display(MapElement Element) { Elements.Add(Element); }
        public void Remove(MapElement Element) { Elements.Remove(Element); }
        public IObservable<MapMouseActionEventArgs> Clicks { get; }
        public void Navigate(EarthPoint ToPoint) { MapCenter = ToPoint; }

        private void PinMarker(bool IsMarkerEnabled)
        {
            if (IsMarkerEnabled) Elements.Add(new MapMarkerElement(MapCenter));
            else
                foreach (var marker in Elements.OfType<MapMarkerElement>().ToList())
                    Elements.Remove(marker);
        }

        private ITileLoader ChooseTileLoader(bool UseHighResolutionTiles)
        {
            return UseHighResolutionTiles
                       ? new WebTileLoader(OsmTilePathProviders.Retina)
                       : new WebTileLoader(OsmTilePathProviders.Default);
        }
    }
}
