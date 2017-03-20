using System;
using System.IO;
using System.Reactive.Linq;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class BlokMapOverlayViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> _fileName;
        private readonly ObservableAsPropertyHelper<bool> _isActive;
        private readonly ObservableAsPropertyHelper<string> _mapConversionDate;
        private readonly ObservableAsPropertyHelper<ushort> _mapNumber;

        public BlokMapOverlayViewModel(TrackSelectorViewModel TrackSelector, BlokMapSearchViewModel Search, IBlokMapService BlokMapService)
        {
            this.TrackSelector = TrackSelector;
            this.Search = Search;

            var mapChanged =
                Observable.FromEventPattern<MapChangedEventArgs>(
                              h => BlokMapService.CurrentMapChanged += h,
                              h => BlokMapService.CurrentMapChanged -= h)
                          .Select(e => new { file = e.EventArgs.FileName, map = BlokMapService.CurrentMap });

            mapChanged.Select(map => map.map != null)
                      .ToProperty(this, x => x.IsActive, out _isActive);

            mapChanged.Select(map => Path.GetFileNameWithoutExtension(map.file))
                      .ToProperty(this, x => x.MapFileName, out _fileName);

            mapChanged.Select(map => map.map.Header.Number)
                      .ToProperty(this, x => x.MapNumber, out _mapNumber);

            mapChanged.Select(map => map.map.Header.ConversionDate.ToShortDateString())
                      .ToProperty(this, x => x.MapConversionDate, out _mapConversionDate);
        }

        public string MapConversionDate
        {
            get { return _mapConversionDate.Value; }
        }

        public ushort MapNumber
        {
            get { return _mapNumber.Value; }
        }

        public string MapFileName
        {
            get { return _fileName.Value; }
        }

        public bool IsActive
        {
            get { return _isActive.Value; }
        }

        public TrackSelectorViewModel TrackSelector { get; private set; }
        public BlokMapSearchViewModel Search { get; private set; }
    }
}
