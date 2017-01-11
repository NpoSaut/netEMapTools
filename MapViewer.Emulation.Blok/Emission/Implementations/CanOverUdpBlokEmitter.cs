using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using BlokFrames;
using Communications.Can;
using Geographics;
using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanOverUdpBlokEmitter : IBlokEmitter
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;
        public CanOverUdpBlokEmitter(ICanPortHandlerProvider CanPortHandlerProvider) { _canPortHandlerProvider = CanPortHandlerProvider; }

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            ICanPortHandler canPortHandler = _canPortHandlerProvider.OpenPort();

            IObservable<long> ipdSampler = Observable.Interval(TimeSpan.FromMilliseconds(500));
            IObservable<long> gpsSampler = Observable.Interval(TimeSpan.FromMilliseconds(1000));

            var sub = new CompositeDisposable(
                canPortHandler,
                Navigation.CombineLatest(ipdSampler, (n, i) => n)
                          .TimeInterval()
                          .Scan(new { disstance = 0.0, speed = 0.0, acceleration = 0.0 },
                                (x, n) => new
                                          {
                                              disstance = x.disstance + 1000 * n.Value.Speed * n.Interval.TotalHours,
                                              speed = n.Value.Speed,
                                              acceleration = 0.8 * (n.Value.Speed - x.speed) + 0.2 * x.acceleration
                                          })
                          .Sample(ipdSampler)
                          .Subscribe(x => EmitSpeed(canPortHandler.Port, x.speed, x.disstance, x.acceleration)),
                Navigation.CombineLatest(gpsSampler, (n, i) => n)
                          .Sample(gpsSampler)
                          .Subscribe(n => EmitPosition(canPortHandler.Port, n.Position, n.Reliability)));

            Navigation.Subscribe(_ => { }, sub.Dispose);
            return Navigation;
        }

        private void EmitSpeed(ICanPort Port, double Speed, double Disstance, double Acceleration)
        {
            var halfsets = new[] { HalfsetKind.SetA, HalfsetKind.SetB };
            foreach (HalfsetKind halfset in halfsets)
            {
                var frame =
                    new IpdState
                    {
                        FrameHalfset = halfset,
                        AccelerationSign = Acceleration > 0 ? IpdState.AccelerationSignKind.Positive : IpdState.AccelerationSignKind.Negative,
                        ActiveSpeedSensor = IpdState.ActiveSpeedSensorKind.Sensor1,
                        Direction = Speed > 0 ? IpdState.DirectionKind.Ahead : IpdState.DirectionKind.Back,
                        EmapPosition = false,
                        IsVirtualCabin = false,
                        LinearOrdinate = (int)Math.Round(Disstance),
                        PassiveSensorImpulseTestState = IpdState.PassiveSensorImpulseTestStateKind.Correct,
                        PassiveSensorSpeedTestState = IpdState.PassiveSensorSpeedTestStateKind.Correct,
                        Speed = (int)Math.Round(Speed),
                        SpeedPulsesAvailable = true,
                        VirtualCabin = IpdState.VirtualCabinKind.Cabin1
                    };
                Port.BeginSend(frame);
            }
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
