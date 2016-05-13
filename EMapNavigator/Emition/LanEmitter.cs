using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using Geographics;

namespace EMapNavigator.Emition
{
    public class LanEmitter : IEmitter
    {
        private static readonly DateTime _time = DateTime.Today.Add(new TimeSpan(15, 0, 0));
        private readonly DateTime _beginTime;
        private readonly IPEndPoint _remodeEp;
        private readonly UdpClient _udpClient;

        public LanEmitter()
        {
            _udpClient = new UdpClient();
            _beginTime = DateTime.Now;
            _remodeEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);
        }

        public void EmitPosition(EarthPoint Position, double Speed)
        {
            var doc = new XDocument(
                new XElement("Information",
                             new XElement("Position",
                                          new XAttribute("Latitude", Position.Latitude.Value),
                                          new XAttribute("Longitude", Position.Longitude.Value)),
                             new XElement("Speed", Speed),
                             new XElement("Time", (_time + (DateTime.Now - _beginTime)))));

            using (var ms = new MemoryStream())
            {
                doc.Save(ms);
                byte[] a = ms.ToArray();
                _udpClient.Send(a, a.Length, _remodeEp);
            }
        }
    }
}
