using System;
using System.IO;
using System.Linq;
using Geographics;
using MapVisualization.Annotations;

namespace Tracking.Formatters
{
    [UsedImplicitly]
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
                    writer.WriteLine("{0:F8}\t{1:F8}", point.Latitude.ToRadian().Value, point.Longitude.ToRadian().Value);
            }
        }

        private EarthPoint LoadPoint(string line)
        {
            string[] parts = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new EarthPoint(new Radian(double.Parse(parts[0])),
                                  new Radian(double.Parse(parts[1])));
        }
    }
}
