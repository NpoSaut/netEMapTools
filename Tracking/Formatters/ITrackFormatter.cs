using System.IO;

namespace Tracking.Formatters
{
    public interface ITrackFormatter
    {
        GpsTrack LoadTrack(Stream input);
        void SaveTrack(GpsTrack GpsTrack, Stream output);
    }
}