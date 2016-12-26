using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Emulation.Blok.Emission.Implementations;
using Microsoft.Practices.Unity;

namespace MapViewer.Emulation.Blok.Modules
{
    public class BlokEmulationServicesModule : UnityContainerModule
    {
        public BlokEmulationServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IBlokEmitterFactory, CanBlockEmitterFactory>("can", new ContainerControlledLifetimeManager())
                .RegisterType<IBlokEmitterFactory, UdpBlokEmitterFactory>("udp", new ContainerControlledLifetimeManager())
                .RegisterType<IAppiDeviceFactory, SingletonAppiDeviceFactory>(new ContainerControlledLifetimeManager());
        }
    }
}
