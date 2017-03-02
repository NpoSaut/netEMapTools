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

            Observable.FromEventPattern(h => BlokMapService.CurrentMapChanged += h,
                                        h => BlokMapService.CurrentMapChanged -= h)
                      .Select(_ => BlokMapService.CurrentMap != null)
                      .ToProperty(this, x => x.IsActive, out _isActive, false);
        }

        public bool IsActive
        {
            get { return _isActive.Value; }
        }

        public TrackSelectorViewModel TrackSelector { get; private set; }
        public BlokMapSearchViewModel Search { get; private set; }
    }
}
