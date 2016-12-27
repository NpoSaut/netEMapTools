using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Communications.Can;
using Communications.Transactions;

namespace MapViewer.Emulation.Blok.Can.UdpCan
{
    public class UdpCanPort : ICanPort
    {
        private readonly UdpClient _client;
        private readonly IDisposable _rxConnection;

        public UdpCanPort(IPEndPoint LocalEnpPoint, IPEndPoint RemoteEndPoint)
        {
            _client = new UdpClient(LocalEnpPoint);
            _client.Connect(RemoteEndPoint);

            IConnectableObservable<ITransaction<CanFrame>> rx =
                Observable.Defer(() => _client.ReceiveAsync().ToObservable())
                          .Repeat()
                          .Select(CreateCanTransaction)
                          .Publish();
            Rx = rx;
            _rxConnection = rx.Connect();

            Options = new UdpCanPortOptions();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _client.Close();
            _rxConnection.Dispose();
        }

        /// <summary>Поток входящих сообщений</summary>
        public IObservable<ITransaction<CanFrame>> Rx { get; private set; }

        /// <summary>Поток исходящих сообщений</summary>
        public IObserver<CanFrame> Tx { get; private set; }

        /// <summary>Опции порта</summary>
        public CanPortOptions Options { get; private set; }

        /// <summary>Начинает отправку кадра</summary>
        /// <param name="Frame">Кадр для отправки</param>
        /// <returns>Транзакция передачи</returns>
        public ITransaction<CanFrame> BeginSend(CanFrame Frame)
        {
            using (var ms = new MemoryStream(Frame.Data.Length + 4))
            {
                var writer = new BinaryWriter(ms);
                writer.Write(Frame.Id);
                writer.Write(Frame.Data);

                byte[] buffer = ms.ToArray();
                _client.Send(buffer, buffer.Length);
                return new InstantaneousTransaction<CanFrame>(Frame);
            }
        }

        private ITransaction<CanFrame> CreateCanTransaction(UdpReceiveResult Packet)
        {
            CanFrame frame = CanFrame.NewWithId(BitConverter.ToInt32(Packet.Buffer, 0),
                                                Packet.Buffer, 4, Packet.Buffer.Length - 4);
            return new InstantaneousTransaction<CanFrame>(frame);
        }

        public class UdpCanPortOptions : CanPortOptions
        {
            /// <summary>Скорость обмена</summary>
            public override int BaudRate { get; set; }
        }
    }
}
