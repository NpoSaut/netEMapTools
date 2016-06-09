using System.Windows.Controls;
using EMapNavigator.ViewModels;

namespace EMapNavigator.Views
{
    /// <summary>Логика взаимодействия для MapView.xaml</summary>
    public partial class MapView : UserControl
    {
        public MapView(MapViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
