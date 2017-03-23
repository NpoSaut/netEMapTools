using Microsoft.Practices.Unity;
using Prism.Regions;

namespace MapViewer
{
    public abstract class UnityContainerInterfaceModule : UnityContainerModule
    {
        protected UnityContainerInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container) { this.RegionManager = RegionManager; }
        protected IRegionManager RegionManager { get; private set; }
    }
}
