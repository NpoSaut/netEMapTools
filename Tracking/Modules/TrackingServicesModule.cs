using MapViewer;
using MapViewer.Mapping;
using Microsoft.Practices.Unity;
using Tracking.Formatters;
using Tracking.Formatters.CanLog;
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
                .RegisterType<ITrackFormatter, CanLogTrackFormatter>("CanLogFormatter", new ContainerControlledLifetimeManager())
                .RegisterType<IPathRider, TrackPathRider>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrackPresenter>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c =>
                                         new TrackFilterPresenterDecorator(
                                             10.0, new SegmentationTrackPresenterDecorator(
                                                       300, new TrackPresenter(c.Resolve<IMappingService>())))))
                .RegisterType<IPathRiderProvider, TrackingControlViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
