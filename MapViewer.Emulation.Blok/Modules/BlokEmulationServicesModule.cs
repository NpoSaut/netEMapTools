using Communications.Appi.Devices;
using Communications.Appi.Factories;
using Communications.Usb;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Emulation.Blok.Emission.Implementations;
using Microsoft.Practices.Unity;
using ReactiveWinUsb;

namespace MapViewer.Emulation.Blok.Modules
{
    public class BlokEmulationServicesModule : UnityContainerModule
    {
        public BlokEmulationServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IUsbFacade, WinUsbFacade>(new ContainerControlledLifetimeManager())
                .RegisterType<IAppiFactory<AppiLine>, AppiBlockFactory>(new ContainerControlledLifetimeManager());

            Container
                .RegisterType<IBlokEmitterFactory, CanBlockEmitterFactory>("can", new ContainerControlledLifetimeManager())
                .RegisterType<IBlokEmitterFactory, UdpBlokEmitterFactory>("udp", new ContainerControlledLifetimeManager())
                .RegisterType<ICanPortHandlerProvider, SingletonCanPortHandlerProvider>(new ContainerControlledLifetimeManager());
        }
    }
}
