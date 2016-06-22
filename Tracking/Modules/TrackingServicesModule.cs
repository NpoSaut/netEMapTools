using MapViewer;
using Microsoft.Practices.Unity;
using Tracking.Formatters;
using Tracking.Presenting;
using Tracking.ViewModels;

namespace Tracking.Modules
{
    public class TrackingServicesModule : UnityContainerModule
    {
        public TrackingServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<ITrackFormatter, GpxTrackFormatter>("GpxTrackFormatter", new ContainerControlledLifetimeManager())
                .RegisterType<ITrackFormatter, PlainRadianTextFormatter>("PlainRadianTextFormatter", new ContainerControlledLifetimeManager())
                .RegisterType<IPathRider, TrackPathRider>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrackPresenter, TrackPresenter>(new ContainerControlledLifetimeManager())
                .RegisterType<IPathRiderProvider, TrackingControlViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
