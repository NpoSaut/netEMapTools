﻿using Communications.Appi.Devices;
using Communications.Appi.Factories;
using Communications.Usb;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Emulation.Blok.Emission.Implementations;
using MapViewer.Emulation.Blok.Emission.Options;
using MapViewer.Emulation.Blok.ViewModels.Options;
using MapViewer.Emulation.Blok.ViewModels.Options.Producing;
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
                    new InjectionFactory(c => new CanBlokEmitterFactory("Через АППИ", new AppiCanPortHandlerProvider(c.Resolve<IAppiFactory<AppiLine>>()))))
                .RegisterType<IBlokEmitterFactory>(
                    "udp-can", new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new CanBlokEmitterFactory("Через UDP-CAN", new UdpCanPortHandlerProvider())))
                .RegisterType<IBlokEmitterFactory, UdpBlokEmitterFactory>("udp", new ContainerControlledLifetimeManager());

            Container
                .RegisterType<IOptionViewModelsSetFactory, OptionViewModelsSetFactory>(new ContainerControlledLifetimeManager())
                .RegisterOptionViewModelFactory<IDescriptorEmissionOption>(c => new DescriptorSelectorViewModel());
        }
    }
}
