namespace BlokMap.ViewModels
{
    public class BlokMapOverlayViewModel
    {
        public BlokMapOverlayViewModel(TrackSelectorViewModel TrackSelector, BlokMapSearchViewModel Search)
        {
            this.TrackSelector = TrackSelector;
            this.Search = Search;
        }

        public TrackSelectorViewModel TrackSelector { get; private set; }
        public BlokMapSearchViewModel Search { get; private set; }
    }
}
