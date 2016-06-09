using System.Windows.Controls;
using BlokMap.ViewModels;

namespace BlokMap.Views
{
    /// <summary>Логика взаимодействия для MapLoaderControlView.xaml</summary>
    public partial class MapLoaderControlView : UserControl
    {
        public MapLoaderControlView(MapLoaderControlViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
