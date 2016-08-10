using MapViewer.Emulation.Blok.Can;
using Microsoft.Practices.Unity;

namespace MapViewer.Emulation.Blok.Modules
{
    public class BlokEmulationServicesModule : UnityContainerModule
    {
        public BlokEmulationServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IBlokEmitter, CanBlokEmitter>(new ContainerControlledLifetimeManager())
                .RegisterType<IAppiDeviceFactory, SingletonAppiDeviceFactory>(new ContainerControlledLifetimeManager());
        }
    }
}
