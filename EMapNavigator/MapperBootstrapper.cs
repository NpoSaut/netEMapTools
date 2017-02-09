using System;
using System.Windows;
using BlokCanTracking.Modules;
using BlokMap.Modules;
using EMapNavigator.Modules;
using MapViewer.Emulation.Blok.Modules;
using MapViewer.Emulation.Modules;
using Microsoft.Practices.Unity;
using MsulEmulation.Modules;
using Prism.Modularity;
using Prism.Unity;
using Tracking.Modules;

namespace EMapNavigator
{
    public class MapperBootstrapper : UnityBootstrapper, IDisposable
    {
        public void Dispose() { Container.Dispose(); }

        protected override void ConfigureModuleCatalog()
        {
            var mc = (ModuleCatalog)ModuleCatalog;

            mc.AddModule(typeof (MappingServicesModule));
            mc.AddModule(typeof (BlokMapServicesModule));
            mc.AddModule(typeof (BlokMapEmulatorServicesModule));
            mc.AddModule(typeof (TrackingServicesModule));
            mc.AddModule(typeof (BlockCanTrackingModule));
            mc.AddModule(typeof (EmulationBasicsServicesModule));
            mc.AddModule(typeof (BlokEmulationServicesModule));
            mc.AddModule(typeof (MsulEmulationServicesModule));

            mc.AddModule(typeof (MappingInterfaceModule));
            mc.AddModule(typeof (BlokMapInterfaceModule));
            mc.AddModule(typeof (MainInterfaceModule));
            mc.AddModule(typeof (TrackingInterfaceModule));
            mc.AddModule(typeof (EmulationBasicsInterfaceModule));
            mc.AddModule(typeof (BlokEmulationInterfaceModule));
            mc.AddModule(typeof (MsulEmulationInterfaceModule));

            base.ConfigureModuleCatalog();
        }

        /// <summary>Creates the shell or main window of the application.</summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        ///     If the returned instance is a <see cref="T:System.Windows.DependencyObject" />, the
        ///     <see cref="T:Prism.Bootstrapper" /> will attach the default <see cref="T:Prism.Regions.IRegionManager" /> of the
        ///     application in its <see cref="F:Prism.Regions.RegionManager.RegionManagerProperty" /> attached property in order to
        ///     be able to add regions by using the <see cref="F:Prism.Regions.RegionManager.RegionNameProperty" />
        ///     attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            var mainWindow = Container.Resolve<MainWindow>();
            return mainWindow;
        }

        /// <summary>Initializes the shell.</summary>
        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
