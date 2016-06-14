using MapViewer;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Tracking.Views;

namespace Tracking.Modules
{
    [ModuleDependency("MappingServicesModule")]
    [ModuleDependency("TrackingServicesModule")]
    public class TrackingInterfaceModule : UnityContainerInterfaceModule
    {
        public TrackingInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            RegionManager
                .RegisterViewWithRegion("MainToolbar", typeof (TrackingControlView));
        }
    }
}
