using System.Windows.Controls;
using EMapNavigator.ViewModels;

namespace EMapNavigator.Views
{
    /// <summary>Логика взаимодействия для MapSettingsView.xaml</summary>
    public partial class MapSettingsView : UserControl
    {
        public MapSettingsView(MapSettingsViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
