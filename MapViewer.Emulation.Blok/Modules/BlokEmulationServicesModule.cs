using System.Collections.Generic;
using Communications;
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
                .RegisterType<IBlokEmitterFactory>(
                    "can", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(
                        c => new CanBlockEmitterFactory("Через АППИ",
                                                        new AppiCanPortHandlerProvider(
                                                            c.Resolve<IAppiFactory<AppiLine>>(),
                                                            AppiLine.Can1,
                                                            new Dictionary<AppiLine, int>
                                                            {
                                                                { AppiLine.Can1, BaudRates.CBR_100K },
                                                                { AppiLine.Can2, BaudRates.CBR_100K },
                                                            }))))
                .RegisterType<IBlokEmitterFactory>(
                    "can-bus", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(
                        c => new CanBlockEmitterFactory("Через АППИ (CAN-BUS)",
                                                        new AppiCanPortHandlerProvider(
                                                            c.Resolve<IAppiFactory<AppiLine>>(),
                                                            AppiLine.Can1,
                                                            new Dictionary<AppiLine, int>
                                                            {
                                                                { AppiLine.Can1, BaudRates.CBR_50K },
                                                                { AppiLine.Can2, BaudRates.CBR_50K },
                                                            }))))
                .RegisterType<IBlokEmitterFactory>(
                    "udp-can", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new CanBlockEmitterFactory("Через UDP-CAN", new UdpCanPortHandlerProvider())))
                .RegisterType<IBlokEmitterFactory, UdpBlokEmitterFactory>("udp", new ContainerControlledLifetimeManager());
        }
    }
}
