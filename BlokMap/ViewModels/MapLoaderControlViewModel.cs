using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using GMapElements;
using MapViewer.Mapping;
using MapViewer.Settings.Interfaces;
using Microsoft.Win32;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class MapLoaderControlViewModel : ReactiveObject
    {
        private readonly IMapBehaviorSettings _behaviorSettings;
        private readonly IBlokMapService _blokMapService;
        private readonly ReactiveCommand<Unit> _load;
        private readonly IMapLoadingService _mapLoadingService;
        private readonly IMappingService _mappingService;

        public MapLoaderControlViewModel(IMapLoadingService MapLoadingService, IMappingService MappingService, IMapBehaviorSettings BehaviorSettings,
                                         IBlokMapService BlokMapService)
        {
            _mapLoadingService = MapLoadingService;
            _mappingService = MappingService;
            _behaviorSettings = BehaviorSettings;
            _blokMapService = BlokMapService;
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
            _blokMapService.SwitchMap(gMap);

            if (_behaviorSettings.JumpOnOpen)
                _mappingService.Navigate(gMap.Sections.First().Posts.First().Point);
        }
    }
}
