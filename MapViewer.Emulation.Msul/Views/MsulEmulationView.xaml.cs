using System.Windows.Controls;
using MapViewer.Emulation.Msul.ViewModels;

namespace MapViewer.Emulation.Msul.Views
{
    /// <summary>Логика взаимодействия для MsulEmulationView.xaml</summary>
    public partial class MsulEmulationView : UserControl
    {
        public MsulEmulationView(MsulEmulationViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
