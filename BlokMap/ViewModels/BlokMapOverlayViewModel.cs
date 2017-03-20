using System.Reactive.Linq;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class BlokMapOverlayViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _isActive;

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
        }

        public bool IsActive
        {
            get { return _isActive.Value; }
        }

        public TrackSelectorViewModel TrackSelector { get; private set; }
        public BlokMapSearchViewModel Search { get; private set; }
    }
}
