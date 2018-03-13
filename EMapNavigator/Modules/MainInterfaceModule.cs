using MapViewer;
using Microsoft.Practices.Unity;

namespace EMapNavigator.Modules
{
    public class MainInterfaceModule : UnityContainerModule
    {
        public MainInterfaceModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IMainMenuService, MainMenuService>(new ContainerControlledLifetimeManager());
        }
    }
}
