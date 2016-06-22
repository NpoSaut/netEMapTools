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

        public string GetFileFilterString()
        {
            return String.Format("All supported|{0}|",
                                 string.Join(";", _formatters.Keys.Select(k => String.Format("*.{0}", k))))
                   +
                   string.Join("|", _formatters.Values.Select(f => string.Format("{0}|*.{1}", f.Name, f.Extension)));
        }

        public ITrackFormatter GetFormatter(string Extension) { return _formatters[Extension.TrimStart('.')]; }
    }
}
