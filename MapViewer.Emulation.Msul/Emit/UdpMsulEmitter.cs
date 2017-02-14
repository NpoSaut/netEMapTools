using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;

namespace MapViewer.Emulation.Msul.Emit
{
    public class UdpMsulEmitter : IMsulEmitter
    {
        private readonly EmissionLink _emissionLink;
        private readonly TimeSpan _messageInterval = TimeSpan.FromMilliseconds(50);

        public UdpMsulEmitter(EmissionLink EmissionLink) { _emissionLink = EmissionLink; }

        public IObservable<MsulMessage> Emit(IObservable<MsulMessage> Messages)
        {
            var client = new UdpClient(new IPEndPoint(_emissionLink.LocalAddress, 125));

            return Observable.Interval(_messageInterval)
                             .CombineLatest(Messages, (i, m) => m)
                             .Sample(_messageInterval)
                             .Select((m, i) => new { m, i = i % m.CarriagesData.Count })
                             .Do(mx => Emit(client, mx.m, mx.i))
                             .Select(mx => mx.m)
                             .Finally(client.Close);
        }

        private int Emit(UdpClient client, MsulMessage Message, int i)
        {
            byte[] data = Message.CommonData.Concat(Message.CarriagesData[i]).ToArray();
            return client.Send(data, data.Length, new IPEndPoint(_emissionLink.TargetAddress, 125));
        }
    }
}
