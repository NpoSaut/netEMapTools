using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using BlokFrames;
using Communications.Can;
using Geographics;
using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanBlokEmitter : IBlokEmitter, IDisposable
    {
        private const int Cogs = 42;
        private const double Diameter = 1250;

        private readonly ICanPortHandlerProvider _canPortHandlerProvider;
        private readonly int _emissionDescriptor;
        private IDisposable _subscription;

        public CanBlokEmitter(ICanPortHandlerProvider CanPortHandlerProvider, int EmissionDescriptor)
        {
            _canPortHandlerProvider = CanPortHandlerProvider;
            _emissionDescriptor = EmissionDescriptor;
        }

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            var canPortHandler = _canPortHandlerProvider.OpenPort();

            var speedSampler = Observable.Interval(TimeSpan.FromMilliseconds(200));
            var gpsSampler = Observable.Interval(TimeSpan.FromMilliseconds(1000));

            _subscription = new CompositeDisposable(
                canPortHandler,
                Navigation.CombineLatest(speedSampler, (n, i) => n)
                          .Sample(speedSampler)
                          .Subscribe(n => EmitSpeed(canPortHandler.Port, n.Speed)),
                Navigation.CombineLatest(gpsSampler, (n, i) => n)
                          .Sample(gpsSampler)
                          .Subscribe(n => EmitPosition(canPortHandler.Port, n.Position, n.Reliability)));

            Navigation.Subscribe(_ => { }, _subscription.Dispose);
            return Navigation;
        }

        public void Dispose() { _subscription.Dispose(); }

        private void EmitSpeed(ICanPort Port, double Speed)
        {
            var frame = new IpdEmulation
                        {
                            Sensor1State = IpdEmulation.SensorState.Get(Speed, Cogs, Diameter, IpdEmulation.SensorState.DpsSensorPlacement.Left),
                            Sensor2State = IpdEmulation.SensorState.Get(Speed, Cogs, Diameter, IpdEmulation.SensorState.DpsSensorPlacement.Left)
                        };
            Port.BeginSend(frame);
        }

        private void EmitPosition(ICanPort Port, EarthPoint Position, bool Reliability)
        {
            var frame = new MmAltLongFrame(Position.Latitude,
                                           Position.Longitude,
                                           Reliability).GetCanFrame();
            var fx = CanFrame.NewWithDescriptor(_emissionDescriptor, frame.Data);
            Port.BeginSend(fx);
            Console.WriteLine("---> {0}", fx);
        }
    }
}
