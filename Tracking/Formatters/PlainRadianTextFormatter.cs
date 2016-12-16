using System;
using System.IO;
using System.Linq;
using Geographics;

namespace Tracking.Formatters
{
    public class PlainRadianTextFormatter : ITrackFormatter
    {
        public FormatterDirection Direction
        {
            get { return FormatterDirection.Save | FormatterDirection.Load; }
        }

        public string Name
        {
            get { return "Radian Plain Text"; }
        }

        public string Extension
        {
            get { return "rad"; }
        }

        public GpsTrack LoadTrack(Stream input)
        {
            var reader = new StreamReader(input);
            return new GpsTrack(
                Enumerable.Range(0, int.MaxValue)
                          .Select(i => reader.ReadLine())
                          .TakeWhile(l => l != null)
                          .Select(LoadPoint)
                          .ToList());
        }

        public void SaveTrack(GpsTrack GpsTrack, Stream output)
        {
            using (var writer = new StreamWriter(output))
            {
                foreach (EarthPoint point in GpsTrack.TrackPoints)
                    writer.WriteLine("{0:F8}\t{1:F8}", point.Longitude.ToRadian().Value, point.Latitude.ToRadian().Value);
            }
        }

        private EarthPoint LoadPoint(string line)
        {
            string[] parts = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new EarthPoint(new Radian(double.Parse(parts[1])),
                                  new Radian(double.Parse(parts[0])));
        }
    }
}
