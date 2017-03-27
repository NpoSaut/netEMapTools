using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MapViewer.Emulation;
using MapViewer.Emulation.Wheels;

namespace Tracking
{
    public class PathNavigator : INavigator, IDisposable, IPathNavigatorConfig, INavigatorConfig
    {
        private readonly IDisposable _navigationConnection;
        private readonly Subject<bool> _reliability;
        private readonly Subject<GpsTrack> _track;

        public PathNavigator(IWheel Wheel)
        {
            _track = new Subject<GpsTrack>();
            _reliability = new Subject<bool>();

            var navigation =
                _track.Select(track => new TrackPathRider(track))
                      .CombineLatest(Wheel.Milage,
                                     _reliability,
                                     (rider, milage, reliability) =>
                                         new NavigationInformation(rider.PointAt(milage), Wheel.Speed, reliability))
                      .Publish();
            Navigation = navigation;
            _navigationConnection = navigation.Connect();
        }

        public void Dispose() { _navigationConnection.Dispose(); }

        public IObservable<NavigationInformation> Navigation { get; }

        public bool Relability
        {
            set { _reliability.OnNext(value); }
        }

        public void ChangeTrack(GpsTrack Track) { _track.OnNext(Track); }
    }
}
