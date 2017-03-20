using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MapViewer;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class BlokMapOverlayViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _isActive;

        public BlokMapOverlayViewModel(TrackSelectorViewModel TrackSelector, BlokMapSearchViewModel Search, IBlokMapService BlokMapService,
                                       IWindowTitleManager WindowTitleManager)
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

            mapChanged.Select(map => string.Format("– {0} [{1:X4}] от {2:d}",
                                                   Path.GetFileNameWithoutExtension(map.file),
                                                   map.map.Header.Number,
                                                   map.map.Header.ConversionDate))
                      .Scan((IDisposable)new CompositeDisposable(),
                            (token, line) =>
                            {
                                token.Dispose();
                                return WindowTitleManager.PutText(1, line);
                            })
                      .Subscribe();
        }

        public bool IsActive
        {
            get { return _isActive.Value; }
        }

        public TrackSelectorViewModel TrackSelector { get; private set; }
        public BlokMapSearchViewModel Search { get; private set; }
    }
}
