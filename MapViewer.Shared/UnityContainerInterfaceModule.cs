using Microsoft.Practices.Unity;
using Prism.Regions;

namespace MapViewer
{
    public abstract class UnityContainerInterfaceModule : UnityContainerModule
    {
        public UnityContainerInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container) { this.RegionManager = RegionManager; }
        public IRegionManager RegionManager { get; private set; }
    }
}
