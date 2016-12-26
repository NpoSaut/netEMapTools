using System;
using System.IO;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Geographics;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class UdpBlokEmitter : IBlokEmitter
    {
        private const int Port = 50325;
        private const string RemoteAddress = "localhost";

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            var udpClient = new UdpClient();
            udpClient.Connect(RemoteAddress, Port);

            IObservable<long> speedSampler = Observable.Interval(TimeSpan.FromMilliseconds(500));
            IObservable<long> gpsSampler = Observable.Interval(TimeSpan.FromMilliseconds(1000));

            var sub = new CompositeDisposable(
                udpClient,
                Navigation.CombineLatest(speedSampler, (n, i) => n)
                          .Sample(speedSampler)
                          .TimeInterval()
                          .Scan(0.0, (dst, tn) => dst + (tn.Interval.TotalHours * tn.Value.Speed * 1000))
                          .Subscribe(disstance => EmitSpeed(udpClient, disstance)),
                Navigation.CombineLatest(gpsSampler, (n, i) => n)
                          .Sample(gpsSampler)
                          .Subscribe(n => EmitPosition(udpClient, n.Position, n.Reliability)));

            Navigation.Subscribe(_ => { }, sub.Dispose);
            return Navigation;
        }

        private void EmitSpeed(UdpClient Client, double Disstance)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);
                writer.Write((byte)PacketType.Speed);
                writer.Write((int)Math.Round(Disstance));
                byte[] dat = ms.ToArray();
                Client.Send(dat, dat.Length);
            }
        }

        private void EmitPosition(UdpClient Client, EarthPoint Position, bool Reliability)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);
                writer.Write((byte)PacketType.Position);
                writer.Write(Position.Latitude);
                writer.Write(Position.Longitude);
                writer.Write((byte)(Reliability ? 1 : 0));
                byte[] dat = ms.ToArray();
                Client.Send(dat, dat.Length);
            }
        }

        private enum PacketType : byte
        {
            Position = 0x01,
            Speed = 0x02
        }
    }
}
