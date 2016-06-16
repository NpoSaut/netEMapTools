using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;

namespace MsulEmulation.Emit
{
    public class UdpMsulEmitter : IMsulEmitter
    {
        private readonly TimeSpan _messageInterval = TimeSpan.FromMilliseconds(1000);

        public IObservable<MsulMessage> Emit(IObservable<MsulMessage> Messages)
        {
            var client = new UdpClient(125);
            return Observable.Interval(_messageInterval)
                             .CombineLatest(Messages, (i, m) => m)
                             .Sample(_messageInterval)
                             .Do(m => Emit(client, m))
                             .Finally(client.Close);
        }

        private int Emit(UdpClient client, MsulMessage Message)
        {
            return client.Send(Message.Data, Message.Data.Length, new IPEndPoint(IPAddress.Broadcast, 125));
        }
    }
}
