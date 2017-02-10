using System.Windows;
using System.Windows.Controls;
using EMapNavigator.ViewModels;

namespace EMapNavigator.Views
{
    /// <summary>Логика взаимодействия для MapView.xaml</summary>
    public partial class MapView : UserControl
    {
        private readonly MapViewModel _viewModel;

        public MapView(MapViewModel ViewModel)
        {
            _viewModel = ViewModel;
            InitializeComponent();
        }

        private void MapView_OnLoaded(object Sender, RoutedEventArgs E)
        {
            DataContext = _viewModel;
        }
    }
}
