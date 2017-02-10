using System;
using MapViewer.Settings.Interfaces;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    public class MapSettingsViewModel : ReactiveObject
    {
        private bool _useHighResolutionTiles;

        public MapSettingsViewModel(IMapAppearanceSettings AppearanceSettings)
        {
            _useHighResolutionTiles = AppearanceSettings.HighResolutionTiles;
            this.WhenAnyValue(x => x.UseHighResolutionTiles).Subscribe(v => AppearanceSettings.HighResolutionTiles = v);
        }

        public bool UseHighResolutionTiles
        {
            get { return _useHighResolutionTiles; }
            set { this.RaiseAndSetIfChanged(ref _useHighResolutionTiles, value); }
        }
    }
}
