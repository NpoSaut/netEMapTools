using EMapNavigator.Settings.Implementations;
using EMapNavigator.Settings.Interfaces;
using MapViewer.Settings.Interfaces;
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

            RegisterSettings<IMapPositionSettings>();
            RegisterSettings<IMapBehaviorSettings>();
        }

        private void RegisterSettings<TSettings>()
        {
            _container.RegisterType<TSettings>(new InjectionFactory(c => c.Resolve<ISettingsFactory<TSettings>>().Produce()));
        }
    }
}
