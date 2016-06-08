﻿using System;
using System.Windows;
using EMapNavigator.Modules;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;

namespace EMapNavigator
{
    public class MapperBootstrapper : UnityBootstrapper, IDisposable
    {
        public void Dispose() { Container.Dispose(); }

        protected override void ConfigureModuleCatalog()
        {
            var mc = (ModuleCatalog)ModuleCatalog;

            mc.AddModule(typeof (MainModule));

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
