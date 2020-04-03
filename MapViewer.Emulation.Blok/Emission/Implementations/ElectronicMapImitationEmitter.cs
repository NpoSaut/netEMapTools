using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using BlokFrames;
using Communications.Can;
using Geographics;
using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class ElectronicMapImitationEmitter : IBlokEmitter
    {
        private const int ImitIpdDateDescriptor = 0x7E8 << 5;
        private const int ImitMmAltLongDescriptor = 0x7E9 << 5;
        private const int ImitMmAltSpMagDescriptor = 0x7EA << 5;
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;

        public ElectronicMapImitationEmitter(ICanPortHandlerProvider CanPortHandlerProvider)
        {
            _canPortHandlerProvider = CanPortHandlerProvider;
        }

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            var canPortHandler = _canPortHandlerProvider.OpenPort();

            var emulationTasks =
                new[]
                {
                    new
                    {
                        interval = 1000,
                        task = (Action<NavigationInformation>) (n =>
                            EmitPosition(canPortHandler.Port, n.Position, n.Reliability))
                    },
                    new
                    {
                        interval = 1000,
                        task = (Action<NavigationInformation>) (n => EmitTime(canPortHandler.Port, DateTime.Now))
                    },
                    new
                    {
                        interval = 1000,
                        task = (Action<NavigationInformation>) (n =>
                            EmitMysteriousStuffWithSpeed(canPortHandler.Port, n.Speed))
                    }
                };

            var sub = new CompositeDisposable(
                emulationTasks
                    .Select(t => new {t.task, sampler = Observable.Interval(TimeSpan.FromMilliseconds(t.interval))})
                    .Select(t => Navigation.CombineLatest(t.sampler, (n, i) => n)
                        .Sample(t.sampler)
                        .Subscribe(t.task))
                    .Concat(new[] {canPortHandler})
                    .ToList());

            Navigation.Subscribe(_ => { }, sub.Dispose);
            return Navigation;
        }

        private void EmitMysteriousStuffWithSpeed(ICanPort Port, double Speed)
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);
            writer.Write((ushort) 0);
            writer.Write((ushort) (0x03ff & (int) Speed));
            writer.Write((byte) 0);
            writer.Write((byte) 0);
            writer.Write((byte) 0x80);
            writer.Write((byte) 0);

            var frame = CanFrame.NewWithDescriptor(ImitMmAltSpMagDescriptor, ms.ToArray());
            Port.BeginSend(frame);
        }

        private void EmitTime(ICanPort Port, DateTime Time)
        {
            var frame = IpdDate.FromTime(Time).GetCanFrame();
            var fx = CanFrame.NewWithDescriptor(ImitIpdDateDescriptor, frame.Data);
            Port.BeginSend(fx);
        }

        private void EmitPosition(ICanPort Port, EarthPoint Position, bool Reliability)
        {
            var frame = new MmAltLongFrame(Position.Latitude,
                Position.Longitude,
                Reliability).GetCanFrame();
            var fx = CanFrame.NewWithDescriptor(ImitMmAltLongDescriptor, frame.Data);
            Port.BeginSend(fx);
        }
    }
}