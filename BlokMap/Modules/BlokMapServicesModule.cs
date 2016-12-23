using BlokMap.ViewModels;
using MapViewer;
using Microsoft.Practices.Unity;

namespace BlokMap.Modules
{
    public class BlokMapServicesModule : UnityContainerModule
    {
        public BlokMapServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IMapLoadingService, MapLoadingService>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrackSource, TrackSelectorViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
