using Communications.Appi.Devices;
using Communications.Appi.Factories;
using Communications.Usb;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Emulation.Blok.Emission.Implementations;
using MapViewer.Emulation.Blok.Emission.Options;
using MapViewer.Emulation.Blok.ViewModels.Options;
using MapViewer.Emulation.Blok.ViewModels.Options.Producing;
using Microsoft.Practices.Unity;

namespace MapViewer.Emulation.Blok.Modules
{
    public class BlokEmulationServicesModule : UnityContainerModule
    {
        public BlokEmulationServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IBlokEmitterFactory>(
                    "can", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new CanBlokEmitterFactory("Через АППИ", new AppiCanPortHandlerProvider())))
                .RegisterType<IBlokEmitterFactory>(
                    "udp-can", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new CanBlokEmitterFactory("Через UDP-CAN", new UdpCanPortHandlerProvider())))
                .RegisterType<IBlokEmitterFactory, UdpBlokEmitterFactory>("udp", new ContainerControlledLifetimeManager())
                .RegisterType<IBlokEmitterFactory>(
                    "can-imitation", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new ElectronicMapImitationEmitterFactory(new AppiCanPortHandlerProvider())));

            Container
                .RegisterType<IOptionViewModelsSetFactory, OptionViewModelsSetFactory>(new ContainerControlledLifetimeManager())
                .RegisterOptionViewModelFactory<IDescriptorEmissionOption>(c => new DescriptorSelectorViewModel());
        }
    }
}
