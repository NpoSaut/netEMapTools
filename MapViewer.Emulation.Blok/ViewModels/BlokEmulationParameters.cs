using System.Reactive.Linq;
using Geographics;
using MapViewer.Emulation.Wheels;
using ReactiveUI;
using Tracking;

namespace MapViewer.Emulation.Blok.ViewModels
{
    public class BlokEmulationParameters : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<EarthPoint> _position;
        private readonly ObservableAsPropertyHelper<double> _speed;

        public BlokEmulationParameters(IPathRiderProvider PathRiderProvider, IWheel Wheel)
        {
            Observable.FromEvent(a => Wheel.SpeedChanged += (s, e) => a(), a => { })
                      .Select(_ => Wheel.Speed)
                      .ToProperty(this, x => x.Speed, out _speed);

            PathRiderProvider.PathRider
                             .CombineLatest(Wheel.Milage,
                                            (rider, milage) => rider.PointAt(milage))
                             .ToProperty(this, x => x.Position, out _position);
        }

        public double Speed
        {
            get { return _speed.Value; }
        }

        public EarthPoint Position
        {
            get { return _position.Value; }
        }
    }
}
