using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Geographics;

namespace Tracking.Formatters
{
    public class GpxTrackFormatter : ITrackFormatter
    {
        public void SaveTrack(GpsTrack GpsTrack, Stream output)
        {
            var doc =
                new XDocument(
                    new XElement("gpx",
                                 new XElement("trk",
                                              new XElement("trkseg",
                                                           GpsTrack.TrackPoints.Select(point =>
                                                                                    new XElement("trkpt",
                                                                                                 new XAttribute("lat",
                                                                                                                point
                                                                                                                    .Latitude),
                                                                                                 new XAttribute("lon",
                                                                                                                point
                                                                                                                    .Longitude)))))));
            doc.Save(output);
        }

        public GpsTrack LoadTrack(Stream input)
        {
            XDocument doc = XDocument.Load(input);
            if (doc.Root == null) throw new ArgumentException("Указанный файл трека пуст");
            return
                new GpsTrack(doc.Root
                             .Elements("trk").First()
                             .Elements("trkseg").First()
                             .Elements("trkpt").Select(XPoint =>
                                                       new EarthPoint((Double)XPoint.Attribute("lat"),
                                                                      (Double)XPoint.Attribute("lon"))).ToList());
        }
    }
}
