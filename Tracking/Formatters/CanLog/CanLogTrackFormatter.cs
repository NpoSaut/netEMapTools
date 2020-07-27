using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Geographics;

namespace Tracking.Formatters.CanLog
{
    public class CanLogTrackFormatter : ITrackFormatter
    {
        private const string _pattern = @"(?<descriptor>[0-9a-fA-F]{4})[\s:]+?((?<databyte>[0-9a-fA-F]{2})\s*){1,8}$";
        private static readonly Regex _regex;

        static CanLogTrackFormatter()
        {
            _regex = new Regex(_pattern, RegexOptions.Compiled);
        }

        public FormatterDirection Direction => FormatterDirection.Load;

        public string Name => "Текстовый Can-лог";

        public string Extension => "txt";

        public GpsTrack LoadTrack(Stream input)
        {
            var targetDescriptor = MmAltLongFrame.Descriptor;

            IList<EarthPoint> trackPoints =
                LoadCanLog(input).AsParallel()
                                 .AsOrdered()
                                 .Select(ParseRecordLine)
                                 .Where(f => f.HasValue)
                                  // ReSharper disable once PossibleInvalidOperationException
                                 .Select(f => f.Value)
                                 .Where(f => f.Descriptor == targetDescriptor)
                                 .Select(f => MmAltLongFrame.Decode(f.Data))
                                 .Where(f => f.Reliable)
                                 .Select(f => new EarthPoint(f.Latitude, f.Longitude))
                                 .ToList();

            return new GpsTrack(trackPoints);
        }

        public void SaveTrack(GpsTrack GpsTrack, Stream output)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> LoadCanLog(Stream input)
        {
            using (TextReader tr = new StreamReader(input))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                    yield return line;
            }
        }

        private static CanFrameLogRecord? ParseRecordLine(string Line)
        {
            var match = _regex.Match(Line);
            if (match.Success)
            {
                var desc = Convert.ToUInt16(match.Groups["descriptor"].Value, 16);
                var c    = match.Groups["databyte"].Captures.OfType<Capture>().ToList();
                var bs   = c.Select(bc => Convert.ToByte(bc.Value, 16)).ToArray();
                return new CanFrameLogRecord(desc, bs);
            }
            else
            {
                Debug.WriteLine("{0}  XXX", Line);
                return null;
            }
        }
    }
}