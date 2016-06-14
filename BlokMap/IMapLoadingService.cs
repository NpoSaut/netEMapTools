using System.Threading.Tasks;
using GMapElements;

namespace BlokMap
{
    public interface IMapLoadingService
    {
        Task<GMap> LoadBlokMap(string FileName);
    }
}