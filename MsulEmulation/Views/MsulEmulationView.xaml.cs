using System.Windows.Controls;
using MsulEmulation.ViewModels;

namespace MsulEmulation.Views
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
