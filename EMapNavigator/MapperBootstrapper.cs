using System;
using System.Windows;
using BlokCanTracking.Modules;
using BlokMap.Modules;
using EMapNavigator.Modules;
using MapViewer;
using MapViewer.Emulation.Blok.Modules;
using MapViewer.Emulation.Modules;
using MapViewer.Environment;
using MapViewer.Environment.Implementations;
using MapViewer.GoogleGeocoding;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using Tracking.Modules;

namespace EMapNavigator
{
    public class MapperBootstrapper : UnityBootstrapper, IDisposable
    {
        private readonly string[] _args;

        public MapperBootstrapper(string[] Args) { _args = Args; }
        public void Dispose() { Container.Dispose(); }

        protected override void ConfigureContainer()
        {
            Container.RegisterInstance<IStartupArgumentsProvider>(new ListStartupArgumentsProvider(_args));
            base.ConfigureContainer();
        }

        protected override void ConfigureModuleCatalog()
        {
            var mc = (ModuleCatalog)ModuleCatalog;

            mc.AddModule(typeof (SharedModule));
            mc.AddModule(typeof (GoogleGeocodingModule));
            mc.AddModule(typeof (SettingsModule));
            mc.AddModule(typeof (MappingServicesModule));
            mc.AddModule(typeof (BlokMapServicesModule));
            mc.AddModule(typeof (BlokMapEmulatorServicesModule));
            mc.AddModule(typeof (TrackingServicesModule));
            mc.AddModule(typeof (BlockCanTrackingModule));
            mc.AddModule(typeof (EmulationBasicsServicesModule));
            mc.AddModule(typeof (BlokEmulationServicesModule));
            //mc.AddModule(typeof (MsulEmulationServicesModule));

            mc.AddModule(typeof (SettingsInterfaceModule));
            mc.AddModule(typeof (BlokMapInterfaceModule));
            mc.AddModule(typeof (MainInterfaceModule));
            mc.AddModule(typeof (TrackingInterfaceModule));
            mc.AddModule(typeof (EmulationBasicsInterfaceModule));
            mc.AddModule(typeof (BlokEmulationInterfaceModule));
            //mc.AddModule(typeof (MsulEmulationInterfaceModule));
            mc.AddModule(typeof (MappingInterfaceModule));

            base.ConfigureModuleCatalog();
        }

        protected override DependencyObject CreateShell()
        {
            var mainWindow = Container.Resolve<MainWindow>();
            return mainWindow;
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
