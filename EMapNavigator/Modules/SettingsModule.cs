using EMapNavigator.Settings.Implementations;
using EMapNavigator.Settings.Interfaces;
using Microsoft.Practices.Unity;
using Prism.Modularity;

namespace EMapNavigator.Modules
{
    public class SettingsModule : IModule
    {
        private readonly IUnityContainer _container;
        public SettingsModule(IUnityContainer Container) { _container = Container; }

        public void Initialize()
        {
            _container
                .RegisterType(typeof (ISettingsFactory<>), typeof (JsonUserSettingsFactory), new ContainerControlledLifetimeManager());

            _container
                .RegisterType<IMapPositionSettings>(new InjectionFactory(c => c.Resolve<ISettingsFactory<IMapPositionSettings>>().Produce()));
        }
    }
}
