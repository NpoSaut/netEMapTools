using System;
using System.Collections.Generic;
using System.Linq;
using Tracking.Formatters;

namespace Tracking
{
    public class TrackFormatterManager
    {
        private readonly IDictionary<string, ITrackFormatter> _formatters;

        public TrackFormatterManager(ITrackFormatter[] Formatters)
        {
            _formatters = Formatters.ToDictionary(f => f.Extension, StringComparer.CurrentCultureIgnoreCase);
        }

        public string GetFileFilterString(FormatterDirection Direction)
        {
            IEnumerable<KeyValuePair<string, ITrackFormatter>> formatters =
                _formatters.Where(f => (f.Value.Direction & Direction) == Direction)
                           .ToList();

            return String.Format("All supported|{0}|",
                                 string.Join(";", formatters.Select(k => String.Format("*.{0}", k.Key))))
                   +
                   string.Join("|", formatters.Select(f => string.Format("{0}|*.{1}", f.Value.Name, f.Value.Extension)));
        }

        public ITrackFormatter GetFormatter(string Extension) { return _formatters[Extension.TrimStart('.')]; }
    }
}
