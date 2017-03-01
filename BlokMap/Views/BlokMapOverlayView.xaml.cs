using System.Windows.Controls;
using BlokMap.ViewModels;

namespace BlokMap.Views
{
    /// <summary>Логика взаимодействия для BlokMapOverlayView.xaml</summary>
    public partial class BlokMapOverlayView : UserControl
    {
        public BlokMapOverlayView(BlokMapOverlayViewModel ViewModel)
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
