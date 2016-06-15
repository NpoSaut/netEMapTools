using MapViewer;
using Microsoft.Practices.Unity;
using Tracking.Formatters;
using Tracking.Presenting;

namespace Tracking.Modules
{
    public class TrackingServicesModule : UnityContainerModule
    {
        public TrackingServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IPathRider, TrackRider>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrackFormatter, GpxTrackFormatter>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrackPresenter, TrackPresenter>(new ContainerControlledLifetimeManager());
        }
    }
}
