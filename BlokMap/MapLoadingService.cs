using System.IO;
using GMapElements;
using Microsoft.Win32;

namespace BlokMap
{
    public class MapLoadingService : IMapLoadingService
    {
        private readonly MapPresenter _mapPresenter;

        public MapLoadingService(MapPresenter MapPresenter) { _mapPresenter = MapPresenter; }

        public void LoadBlokMap()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
                Load(dlg.FileName);
        }

        private void Load(string FileName)
        {
            GMap gMap;
            using (var mapStream = new FileStream(FileName, FileMode.Open))
            {
                gMap = GMap.Load(mapStream);
            }

            _mapPresenter.PrintPosts(gMap);
            _mapPresenter.PrintObjects(gMap, 1);
        }
    }
}
