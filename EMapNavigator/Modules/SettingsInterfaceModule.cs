using EMapNavigator.Views;
using MapViewer;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace EMapNavigator.Modules
{
    public class SettingsInterfaceModule : UnityContainerInterfaceModule
    {
        public SettingsInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize() { RegionManager.RegisterViewWithRegion("MainToolbar", typeof (SettingsToolbarView)); }
    }
}
