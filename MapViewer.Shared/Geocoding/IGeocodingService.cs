using System.Threading.Tasks;
using Geographics;

namespace MapViewer.Geocoding
{
    public interface IGeocodingService
    {
        Task<string> GetCity(EarthPoint Point);
    }
}