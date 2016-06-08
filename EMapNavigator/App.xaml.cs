using System.Windows;

namespace EMapNavigator
{
    /// <summary>Логика взаимодействия для App.xaml</summary>
    public partial class App : Application
    {
        private MapperBootstrapper _bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _bootstrapper = new MapperBootstrapper();
            _bootstrapper.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _bootstrapper.Dispose();
        }
    }
}
