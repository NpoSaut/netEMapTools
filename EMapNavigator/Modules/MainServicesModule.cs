using MapViewer;
using Microsoft.Practices.Unity;

namespace EMapNavigator.Modules
{
    public class MainServicesModule : UnityContainerModule
    {
        public MainServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IWindowTitleManager, WindowTitleManager>(new ContainerControlledLifetimeManager());
        }
    }
}
