using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MapViewer.Emulation;
using MapViewer.Emulation.Wheels;

namespace Tracking
{
    public class PathNavigator : INavigator, IDisposable
    {
        private readonly IDisposable _navigationConnection;
        private readonly Subject<GpsTrack> _track;

        public PathNavigator(IWheel Wheel)
        {
            _track = new Subject<GpsTrack>();
            var navigation =
                _track.Select(track => new TrackPathRider(track))
                      .CombineLatest(Wheel.Milage,
                                     (rider, milage) => new NavigationInformation(rider.PointAt(milage), Wheel.Speed, true))
                      .Publish();
            Navigation = navigation;
            _navigationConnection = navigation.Connect();
        }

        public void Dispose() { _navigationConnection.Dispose(); }

        public IObservable<NavigationInformation> Navigation { get; }
        public void ChangeTrack(GpsTrack Track) { _track.OnNext(Track); }
    }
}
