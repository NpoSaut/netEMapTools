using System.Windows.Controls;
using MapViewer.Emulation.ViewModels;

namespace MapViewer.Emulation.Views
{
    /// <summary>Логика взаимодействия для SpeedControlView.xaml</summary>
    public partial class SpeedControlView : UserControl
    {
        public SpeedControlView(SpeedControlViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
