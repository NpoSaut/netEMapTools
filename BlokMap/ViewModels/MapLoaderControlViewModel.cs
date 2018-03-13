using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
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
        private readonly IMappingService _mappingService;

        public MapLoaderControlViewModel(IMappingService MappingService, IMapBehaviorSettings BehaviorSettings, IBlokMapService BlokMapService)
        {
            _mappingService   = MappingService;
            _behaviorSettings = BehaviorSettings;
            _blokMapService   = BlokMapService;
            Load              = ReactiveCommand.CreateAsyncTask((_, c) => LoadMap());
            Export            = ReactiveCommand.CreateAsyncTask((_, c) => ExportMap());
        }

        public ICommand Load   { get; }
        public ICommand Export { get; }

        private async Task LoadMap()
        {
            var dlg = new OpenFileDialog { DefaultExt = "gps", Filter = "Файл электронной карты|*.gps|Все файлы|*.*", FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            await _blokMapService.LoadMap(dlg.FileName);

            if (_behaviorSettings.JumpOnOpen)
                _mappingService.Navigate(_blokMapService.CurrentMap.Sections.First().Posts.First().Point);
        }

        private async Task ExportMap()
        {
            var dlg = new SaveFileDialog { DefaultExt = "gpx", Filter = "Файл gpx|*.gpx", FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            await Task.Run(() => ExportToGpx(_blokMapService.CurrentMap, dlg.FileName));
        }

        private void ExportToGpx(GMap Map, string FileName)
        {
            var doc =
                new XDocument(
                    new XElement("gpx",
                                 Map.Sections
                                    .SelectMany(t => t.Posts)
                                    .Select(p => new XElement("wpt",
                                                              new XAttribute("lat", p.Point.Latitude.Value),
                                                              new XAttribute("lon", p.Point.Longitude.Value),
                                                              new XElement("name", p.Ordinate.ToString())))));
            doc.Save(FileName);
        }
    }
}
