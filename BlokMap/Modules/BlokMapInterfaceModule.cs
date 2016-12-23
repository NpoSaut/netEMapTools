using BlokMap.Views;
using MapViewer;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace BlokMap.Modules
{
    [ModuleDependency("MappingServicesModule")]
    [ModuleDependency("BlokMapServicesModule")]
    public class BlokMapInterfaceModule : UnityContainerInterfaceModule
    {
        public BlokMapInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container, RegionManager) { }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion("MainToolbar", typeof (MapLoaderControlView));
            RegionManager.RegisterViewWithRegion("MapOverlay", typeof (TrackSelectorView));
        }
    }
}
