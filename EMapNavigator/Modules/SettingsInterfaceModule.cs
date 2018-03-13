using MapViewer;
using MapViewer.Settings.Interfaces;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace EMapNavigator.Modules
{
    public class SettingsInterfaceModule : UnityContainerInterfaceModule
    {
        public SettingsInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            var menu               = Container.Resolve<IMainMenuService>();
            var appearanceSettings = Container.Resolve<IMapAppearanceSettings>();
            var behaviorSettings   = Container.Resolve<IMapBehaviorSettings>();

            menu.RegisterCheckbox((MenuPath)"Опции" / "Карты высокого разрешения",
                                  () => appearanceSettings.HighResolutionTiles,
                                  v => appearanceSettings.HighResolutionTiles = v);

            menu.RegisterCheckbox((MenuPath)"Опции" / "Перескакивать при открытии",
                                  () => behaviorSettings.JumpOnOpen,
                                  v => behaviorSettings.JumpOnOpen = v);
        }
    }
}
