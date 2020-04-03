using System.Threading;
using System.Threading.Tasks;
using Geographics;

namespace MapViewer.Geocoding
{
    public interface IGeocodingService
    {
        Task<string> GetPlacementName(EarthPoint Point, CancellationToken token);
    }
}