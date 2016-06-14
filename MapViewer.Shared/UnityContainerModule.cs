using Microsoft.Practices.Unity;
using Prism.Modularity;

namespace MapViewer
{
    public abstract class UnityContainerModule : IModule
    {
        protected UnityContainerModule(IUnityContainer Container) { this.Container = Container; }
        protected IUnityContainer Container { get; private set; }

        public abstract void Initialize();
    }
}
