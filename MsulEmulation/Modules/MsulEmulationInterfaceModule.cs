using MapViewer;
using Microsoft.Practices.Unity;
using MsulEmulation.Views;
using Prism.Modularity;
using Prism.Regions;

namespace MsulEmulation.Modules
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
