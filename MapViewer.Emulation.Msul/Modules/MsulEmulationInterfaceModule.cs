using MapViewer.Emulation.Msul.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace MapViewer.Emulation.Msul.Modules
{
    [ModuleDependency("MsulEmulationServicesModule")]
    public class MsulEmulationInterfaceModule : UnityContainerInterfaceModule
    {
        public MsulEmulationInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            RegionManager
                .RegisterViewWithRegion("Emulators", typeof (MsulEmulationView));
        }
    }
}
