using MapViewer.Geocoding;
using Microsoft.Practices.Unity;

namespace MapViewer.GoogleGeocoding
{
    public class GoogleGeocodingModule : UnityContainerModule
    {
        public GoogleGeocodingModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IGeocodingService, GoogleGeocodingService>(new ContainerControlledLifetimeManager());
        }
    }
}
