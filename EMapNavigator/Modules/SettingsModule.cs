using System;
using System.Collections.Generic;
using System.Linq;
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
            RegisterSettingsInstance<UserSettings>("Settings.json");
            RegisterSettingsInstance<MsulEmulationSettings>("MsulSettings.json");
        }

        private void RegisterSettingsInstance<TInstance>(string FileName) where TInstance : ISettings, new()
        {
            _container
                .RegisterType(typeof (ISettingsFactory<TInstance>), typeof (JsonSettingsFactory<TInstance>),
                              new ContainerControlledLifetimeManager(),
                              new InjectionConstructor(FileName));

            Type settingsInterface = typeof (ISettings);
            List<Type> interfaces = typeof (TInstance).GetInterfaces()
                                                      .Where(settingsInterface.IsAssignableFrom)
                                                      .ToList();

            foreach (Type implementedInterface in interfaces)
            {
                _container.RegisterType(implementedInterface,
                                        new InjectionFactory(c => c.Resolve<ISettingsFactory<TInstance>>().Produce()));
            }
        }
    }
}
