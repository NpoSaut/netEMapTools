using System.Windows.Controls;
using MapViewer.Emulation.ViewModels;

namespace MapViewer.Emulation.Views
{
    /// <summary>Логика взаимодействия для NavigationControlView.xaml</summary>
    public partial class NavigationControlView : UserControl
    {
        public NavigationControlView(NavigationControlViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
