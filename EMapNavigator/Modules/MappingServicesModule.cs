using EMapNavigator.ViewModels;
using MapViewer;
using MapViewer.Mapping;
using Microsoft.Practices.Unity;

namespace EMapNavigator.Modules
{
    public class MappingServicesModule : UnityContainerModule
    {
        public MappingServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IMappingService, MapViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
