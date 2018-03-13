using BlokMap.ViewModels;
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
            RegionManager.RegisterViewWithRegion("MapOverlay", typeof(BlokMapOverlayView));

            var menu      = Container.Resolve<IMainMenuService>();
            var viewModel = Container.Resolve<MapLoaderControlViewModel>();

            menu.RegisterCommand((MenuPath)"Карта" / "Открыть карту...",
                                 viewModel.Load);
        }
    }
}
