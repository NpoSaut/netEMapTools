using System.Windows.Input;
using Prism.Commands;

namespace BlokMap.ViewModels
{
    public class MapLoaderControlViewModel
    {
        private readonly IMapLoadingService _mapLoadingService;

        public MapLoaderControlViewModel(IMapLoadingService MapLoadingService)
        {
            _mapLoadingService = MapLoadingService;
            Load = new DelegateCommand(LoadMap);
        }

        public ICommand Load { get; private set; }

        private void LoadMap() { _mapLoadingService.LoadBlokMap(); }
    }
}
