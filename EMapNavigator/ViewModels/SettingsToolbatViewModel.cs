using System.Windows;
using System.Windows.Input;
using EMapNavigator.Views;
using MapVisualization.Annotations;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    [UsedImplicitly]
    public class SettingsToolbatViewModel : ReactiveObject
    {
        public SettingsToolbatViewModel()
        {
            ShowSettingsDialog = ReactiveCommand.Create(() => new SettingsWindow { Owner = Application.Current.MainWindow }.Show());
        }

        public ICommand ShowSettingsDialog { get; private set; }
    }
}
