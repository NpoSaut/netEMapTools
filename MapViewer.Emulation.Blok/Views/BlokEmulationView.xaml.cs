using System.Windows.Controls;
using MapViewer.Emulation.Blok.ViewModels;

namespace MapViewer.Emulation.Blok.Views
{
    /// <summary>Логика взаимодействия для BlokEmulationView.xaml</summary>
    public partial class BlokEmulationView : UserControl
    {
        public BlokEmulationView(BlokEmulationViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
