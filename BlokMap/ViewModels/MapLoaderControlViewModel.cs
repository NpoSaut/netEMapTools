using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using GMapElements;
using MapViewer.Mapping;
using Microsoft.Win32;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class MapLoaderControlViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit> _load;
        private readonly IMapLoadingService _mapLoadingService;
        private readonly MapPresenter _mapPresenter;
        private readonly IMappingService _mappingService;

        public MapLoaderControlViewModel(IMapLoadingService MapLoadingService, MapPresenter MapPresenter, IMappingService MappingService)
        {
            _mapLoadingService = MapLoadingService;
            _mapPresenter = MapPresenter;
            _mappingService = MappingService;
            _load = ReactiveCommand.CreateAsyncTask((_, c) => LoadMap());
        }

        public ICommand Load
        {
            get { return _load; }
        }

        private async Task LoadMap()
        {
            var dlg = new OpenFileDialog { DefaultExt = "gpx", Filter = "Файл электронной карты|*.gps|Все файлы|*.*", FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            GMap gMap = await _mapLoadingService.LoadBlokMap(dlg.FileName);

            _mapPresenter.PrintPosts(gMap);
            _mapPresenter.PrintObjects(gMap, 1);
            _mappingService.Navigate(gMap.Sections.First().Posts.First().Point);
        }
    }
}
