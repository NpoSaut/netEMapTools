using MapViewer.Emulation.Views;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace MapViewer.Emulation.Modules
{
    public class EmulationBasicsInterfaceModule : UnityContainerInterfaceModule
    {
        public EmulationBasicsInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            RegionManager
                .RegisterViewWithRegion("MapOverlay", typeof (NavigationControlView));
        }
    }
}
