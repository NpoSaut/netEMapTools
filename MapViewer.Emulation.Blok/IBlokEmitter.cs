using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using BlokFrames;
using Communications.Appi;
using Communications.Can;
using Geographics;
using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok
{
    public interface IBlokEmitter
    {
        IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation);
    }

    public static class BlokEmitterHelper
    {
        public static IObservable<NavigationInformation> EmitThorough(this IObservable<NavigationInformation> Navigation, IBlokEmitter Emitter)
        {
            return Emitter.Emit(Navigation);
        }
    }

    public class NavigationInformation
    {
        public NavigationInformation(EarthPoint Position, double Speed, bool Reliability)
        {
            this.Reliability = Reliability;
            this.Position = Position;
            this.Speed = Speed;
        }

        public EarthPoint Position { get; private set; }
        public double Speed { get; private set; }
        public bool Reliability { get; private set; }
    }

    public class CanBlokEmitter : IBlokEmitter
    {
        private const int Cogs = 42;
        private const double Diameter = 1250;

        private readonly IAppiDeviceFactory _appiDeviceFactory;
        public CanBlokEmitter(IAppiDeviceFactory AppiDeviceFactory) { _appiDeviceFactory = AppiDeviceFactory; }

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            IAppiHandler appi = _appiDeviceFactory.GetDevice();

            IObservable<long> speedSampler = Observable.Interval(TimeSpan.FromMilliseconds(200));
            IObservable<long> gpsSampler = Observable.Interval(TimeSpan.FromMilliseconds(1000));

            var sub = new CompositeDisposable(
                appi,
                Navigation.CombineLatest(speedSampler, (n, i) => n)
                          .Sample(speedSampler)
                          .Subscribe(n => EmitSpeed(appi.Dev, n.Speed)),
                Navigation.CombineLatest(gpsSampler, (n, i) => n)
                          .Sample(gpsSampler)
                          .Subscribe(n => EmitPosition(appi.Dev, n.Position, n.Reliability)));

            Navigation.Subscribe(_ => { }, sub.Dispose);
            return Navigation;
        }

        private void EmitSpeed(AppiDev Appi, double Speed)
        {
            var frame = new IpdEmulation
                        {
                            Sensor1State = IpdEmulation.SensorState.Get(Speed, Cogs, Diameter, IpdEmulation.SensorState.DpsSensorPlacement.Left),
                            Sensor2State = IpdEmulation.SensorState.Get(Speed, Cogs, Diameter, IpdEmulation.SensorState.DpsSensorPlacement.Left)
                        };
            Appi.CanPorts[AppiLine.Can1].Send(frame);
        }

        private void EmitPosition(AppiDev Appi, EarthPoint Position, bool Reliability)
        {
            CanFrame frame = new MmAltLongFrame(Position.Latitude,
                                                Position.Longitude,
                                                Reliability).GetCanFrame();
            CanFrame fx = CanFrame.NewWithId(0x5c0, frame.Data);
            Appi.CanPorts[AppiLine.Can1].Send(fx);
        }
    }
}
