using System;
using System.IO;

namespace Tracking.Formatters
{
    [Flags]
    public enum FormatterDirection
    {
        Save,
        Load
    }

    public interface ITrackFormatter
    {
        FormatterDirection Direction { get; }
        string Name { get; }
        string Extension { get; }

        GpsTrack LoadTrack(Stream input);
        void SaveTrack(GpsTrack GpsTrack, Stream output);
    }
}
