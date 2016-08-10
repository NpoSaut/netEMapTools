using MapViewer.Emulation.Blok.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace MapViewer.Emulation.Blok.Modules
{
    [ModuleDependency("MsulEmulationServicesModule")]
    public class BlokEmulationInterfaceModule : UnityContainerInterfaceModule
    {
        public BlokEmulationInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            RegionManager
                .RegisterViewWithRegion("Emulators", typeof (BlokEmulationView));
        }
    }
}
