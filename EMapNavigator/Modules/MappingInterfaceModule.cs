using EMapNavigator.Views;
using MapViewer;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace EMapNavigator.Modules
{
    public class MappingInterfaceModule : UnityContainerInterfaceModule
    {
        public MappingInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion("Map", typeof (MapView));
            RegionManager.RegisterViewWithRegion("MainToolbar", typeof (MappingToolbarView));
            RegionManager.RegisterViewWithRegion("Settings", typeof (MapSettingsView));
        }
    }
}
