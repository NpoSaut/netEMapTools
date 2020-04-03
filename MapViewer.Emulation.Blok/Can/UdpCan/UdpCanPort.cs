using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Communications;
using Communications.Can;
using Communications.Transactions;
using Communications.Transactions.Results;

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

            var rx =
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

                var buffer = ms.ToArray();
                _client.Send(buffer, buffer.Length);
                return Transaction.WithResult(Frame);
            }
        }

        private ITransaction<CanFrame> CreateCanTransaction(UdpReceiveResult Packet)
        {
            var frame = CanFrame.NewWithId(BitConverter.ToInt32(Packet.Buffer, 0),
                Packet.Buffer, 4, Packet.Buffer.Length - 4);
            return Transaction.WithResult(frame);
        }

        public class UdpCanPortOptions : CanPortOptions
        {
            /// <summary>Скорость обмена</summary>
            public override int BaudRate { get; set; }

            public UdpCanPortOptions() : base(false, false)
            {
            }
        }

        public bool IsClosed => false;
        public event EventHandler Disconnected;

        public void Close()
        {
        }

        public ICanPortOptions Options { get; }
    }
}