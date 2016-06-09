using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace MapViewer
{
    public abstract class UnityContainerModule : IModule
    {
        protected UnityContainerModule(IUnityContainer Container) { this.Container = Container; }
        protected IUnityContainer Container { get; private set; }

        public abstract void Initialize();
    }

    public abstract class UnityContainerInterfaceModule : UnityContainerModule
    {
        public UnityContainerInterfaceModule(IUnityContainer Container, IRegionManager RegionManager) : base(Container) { this.RegionManager = RegionManager; }
        public IRegionManager RegionManager { get; private set; }
    }
}
