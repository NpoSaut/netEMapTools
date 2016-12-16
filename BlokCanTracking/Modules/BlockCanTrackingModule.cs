using BlokCanTracking.Formatters;
using MapViewer;
using Microsoft.Practices.Unity;
using Tracking.Formatters;

namespace BlokCanTracking.Modules
{
    public class BlockCanTrackingModule : UnityContainerModule
    {
        public BlockCanTrackingModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<ITrackFormatter, CanLogTrackFormatter>("CanLogTrackFormatter", new ContainerControlledLifetimeManager());
        }
    }
}
