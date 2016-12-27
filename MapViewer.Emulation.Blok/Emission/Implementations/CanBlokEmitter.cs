using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using BlokFrames;
using Communications.Can;
using Geographics;
using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanBlokEmitter : IBlokEmitter
    {
        private const int Cogs = 42;
        private const double Diameter = 1250;

        private readonly ICanPortHandlerProvider _canPortHandlerProvider;
        public CanBlokEmitter(ICanPortHandlerProvider CanPortHandlerProvider) { _canPortHandlerProvider = CanPortHandlerProvider; }

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            ICanPortHandler canPortHandler = _canPortHandlerProvider.GetDevice();

            IObservable<long> speedSampler = Observable.Interval(TimeSpan.FromMilliseconds(200));
            IObservable<long> gpsSampler = Observable.Interval(TimeSpan.FromMilliseconds(1000));

            var sub = new CompositeDisposable(
                canPortHandler,
                Navigation.CombineLatest(speedSampler, (n, i) => n)
                          .Sample(speedSampler)
                          .Subscribe(n => EmitSpeed(canPortHandler.Port, n.Speed)),
                Navigation.CombineLatest(gpsSampler, (n, i) => n)
                          .Sample(gpsSampler)
                          .Subscribe(n => EmitPosition(canPortHandler.Port, n.Position, n.Reliability)));

            Navigation.Subscribe(_ => { }, sub.Dispose);
            return Navigation;
        }

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
            CanFrame frame = new MmAltLongFrame(Position.Latitude,
                                                Position.Longitude,
                                                Reliability).GetCanFrame();
            CanFrame fx = CanFrame.NewWithId(0x5c0, frame.Data);
            Port.BeginSend(fx);
        }
    }
}
