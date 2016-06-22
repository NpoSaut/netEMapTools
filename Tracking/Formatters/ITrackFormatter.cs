using System.IO;

namespace Tracking.Formatters
{
    public interface ITrackFormatter
    {
        string Name { get; }
        string Extension { get; }

        GpsTrack LoadTrack(Stream input);
        void SaveTrack(GpsTrack GpsTrack, Stream output);
    }
}