using BlokMap.Search.Implementations;
using BlokMap.Search.Interfaces;
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
                .RegisterType<IBlokMapService, BlokMapService>(new ContainerControlledLifetimeManager())
                .RegisterType<IMapLoadingService, MapLoadingService>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrackSource, TrackSelectorViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISearchProvider, OrdinateSearchProvider>(new ContainerControlledLifetimeManager());
        }
    }
}
