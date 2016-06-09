using EMapNavigator.Views;
using MapViewer;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace EMapNavigator.Modules
{
    public class BlokMapEmulatorServicesModule : UnityContainerModule
    {
        public BlokMapEmulatorServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            var rm = Container.Resolve<RegionManager>();
            rm.RegisterViewWithRegion("MapOverlay", typeof (BlokMapEmulatorView));
        }
    }
}
