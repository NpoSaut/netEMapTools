using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BlokFrames;
using Communications.Can;
using Geographics;
using Tracking;
using Tracking.Formatters;

namespace BlokCanTracking.Formatters
{
    public class CanLogTrackFormatter : ITrackFormatter
    {
        private const string Pattern = @"(?<descriptor>[0-9a-fA-F]{4})[\s:]+?((?<databyte>[0-9a-fA-F]{2})\s*){1,8}$";

        public FormatterDirection Direction
        {
            get { return FormatterDirection.Load; }
        }

        public string Name
        {
            get { return "Текстовый Can-лог"; }
        }

        public string Extension
        {
            get { return "txt"; }
        }

        public GpsTrack LoadTrack(Stream input)
        {
            ICollection<int> targetDescriptors = BlokFrame.GetDescriptors<MmAltLongFrame>().Values;

            IList<EarthPoint> trackPoints =
                LoadCanLog(input).Where(f => targetDescriptors.Contains(f.Descriptor))
                                 .Select(BlokFrame.GetBlokFrame<MmAltLongFrame>)
                                 .Where(f => f.Reliable)
                                 .Select(f => new EarthPoint(f.Latitude, f.Longitude))
                                 .ToList();

            return new GpsTrack(trackPoints);
        }

        public void SaveTrack(GpsTrack GpsTrack, Stream output) { throw new NotImplementedException(); }

        private static IEnumerable<CanFrame> LoadCanLog(Stream input)
        {
            var regex = new Regex(Pattern, RegexOptions.Compiled);
            using (TextReader tr = new StreamReader(input))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        ushort desc = Convert.ToUInt16(match.Groups["descriptor"].Value, 16);
                        List<Capture> c = match.Groups["databyte"].Captures.OfType<Capture>().ToList();
                        byte[] bs = c.Select(bc => Convert.ToByte(bc.Value, 16)).ToArray();
                        yield return CanFrame.NewWithDescriptor(desc, bs);
                    }
                    else
                        Debug.WriteLine("{0}  XXX", line);
                }
            }
        }
    }
}
