using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GMapElements;
using MapViewer.Mapping;
using MapVisualization.Elements;
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
        private readonly ITrackSource _trackSource;

        public MapLoaderControlViewModel(IMapLoadingService MapLoadingService, MapPresenter MapPresenter, IMappingService MappingService,
                                         ITrackSource TrackSource)
        {
            _mapLoadingService = MapLoadingService;
            _mapPresenter = MapPresenter;
            _mappingService = MappingService;
            _trackSource = TrackSource;
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

            List<MapElement> posts = _mapPresenter.PrintPosts(gMap).ToList();
            _mappingService.Display(posts);

            _trackSource.Track
                        .Select(t => _mapPresenter.PrintObjects(gMap, t).ToList())
                        .Scan(new { prev = new List<MapElement>(), current = new List<MapElement>() },
                              (a, n) => new { prev = a.current, current = n })
                        .Subscribe(x =>
                                   {
                                       _mappingService.Remove(x.prev);
                                       _mappingService.Display(x.current);
                                   });

            //List<MapElement> objects = _mapPresenter.PrintObjects(gMap, 1).ToList();
            _mappingService.Navigate(gMap.Sections.First().Posts.First().Point);
        }
    }
}
