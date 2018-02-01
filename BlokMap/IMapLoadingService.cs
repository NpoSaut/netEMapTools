using System.Threading.Tasks;
using GMapElements;
using GMapElements.Entities;

namespace BlokMap
{
    public interface IMapLoadingService
    {
        Task<GMap> LoadBlokMap(string FileName);
    }
}