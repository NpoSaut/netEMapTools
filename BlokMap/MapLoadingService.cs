using System.IO;
using System.Threading.Tasks;
using GMapElements;

namespace BlokMap
{
    public class MapLoadingService : IMapLoadingService
    {
        public async Task<GMap> LoadBlokMap(string FileName)
        {
            return await
                   Task.Run(() =>
                            {
                                using (var mapStream = new FileStream(FileName, FileMode.Open))
                                {
                                    return GMap.Load(mapStream);
                                }
                            });
        }
    }
}
