using System;
using MapViewer.Emulation.Blok.Emission.Options;
using Microsoft.Practices.Unity;

namespace MapViewer.Emulation.Blok.ViewModels.Options.Producing
{
    public static class EmissionOptionViewModelFactoryRegistrationHelper
    {
        public static IUnityContainer RegisterOptionViewModelFactory<TOption>(this IUnityContainer Container, Func<IUnityContainer, TOption> Factory)
            where TOption : IEmissionOption
        {
            return Container
                .RegisterType<IEmissionOptionViewModelFactory>(
                    typeof (TOption).FullName,
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new EmissionOptionViewModelFactory<TOption>(() => Factory(c))));
        }
    }
}
