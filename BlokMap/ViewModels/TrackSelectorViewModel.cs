using System;
using System.Collections.Generic;
using System.Linq;
using MapVisualization.Annotations;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    [UsedImplicitly]
    public class TrackSelectorViewModel : ReactiveObject, ITrackSource
    {
        private int _selectedTrack;

        public TrackSelectorViewModel()
        {
            Tracks =
                new[]
                {
                    new[] { new TrackViewModel("0", 0) },
                    Enumerable.Range(1, 15).Select(i => new TrackViewModel(string.Format("{0}П", i), i)),
                    Enumerable.Range(1, 15).Select(i => new TrackViewModel(string.Format("{0}Н", i), 15 + i))
                }
                    .SelectMany(x => x)
                    .ToList();
        }

        public IList<TrackViewModel> Tracks { get; private set; }

        public int SelectedTrack
        {
            get { return _selectedTrack; }
            set { this.RaiseAndSetIfChanged(ref _selectedTrack, value); }
        }

        IObservable<int> ITrackSource.Track
        {
            get { return this.WhenAnyValue(x => x.SelectedTrack); }
        }

        public class TrackViewModel
        {
            public TrackViewModel(string Name, int Number)
            {
                this.Name = Name;
                this.Number = Number;
            }

            public string Name { get; private set; }
            public int Number { get; private set; }
        }
    }
}
