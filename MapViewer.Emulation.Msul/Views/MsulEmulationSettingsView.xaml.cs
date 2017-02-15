using System.Windows.Controls;
using MapViewer.Emulation.Msul.ViewModels;

namespace MapViewer.Emulation.Msul.Views
{
    /// <summary>Логика взаимодействия для MsulEmulationSettingsView.xaml</summary>
    public partial class MsulEmulationSettingsView : UserControl
    {
        public MsulEmulationSettingsView(MsulEmulationSettingsViewModel ViewModel)
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
