using System.Windows.Controls;
using EMapNavigator.ViewModels;

namespace EMapNavigator.Views
{
    /// <summary>Логика взаимодействия для SettingsToolbarView.xaml</summary>
    public partial class SettingsToolbarView : UserControl
    {
        public SettingsToolbarView(SettingsToolbatViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
