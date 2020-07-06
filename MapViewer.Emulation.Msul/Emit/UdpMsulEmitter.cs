using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using MapViewer.Emulation.Msul.Encoding;

namespace MapViewer.Emulation.Msul.Emit
{
    public class UdpMsulEmitter : IMsulEmitter
    {
        private readonly EmissionLink _emissionLink;
        private readonly IMsulMessageEncoder _encoder;
        private readonly TimeSpan _messageInterval = TimeSpan.FromMilliseconds(50);

        public UdpMsulEmitter(EmissionLink EmissionLink, IMsulMessageEncoder Encoder)
        {
            _emissionLink = EmissionLink;
            _encoder      = Encoder;
        }

        public IDisposable Emit(IObservable<MsulData> Data)
        {
            var client = new UdpClient(new IPEndPoint(_emissionLink.LocalAddress, 0));

            return Observable.Interval(_messageInterval)
                             .CombineLatest(Data, (i, m) => m)
                             .Sample(_messageInterval)
                             .Select((m, i) => _encoder.GetMessage(m, i))
                             .Finally(client.Close)
                             .Subscribe(m => Emit(client, m));
        }

        private void Emit(UdpClient client, byte[] Message)
        {
            client.Send(Message, Message.Length, new IPEndPoint(_emissionLink.TargetAddress, 125));
        }
    }
}