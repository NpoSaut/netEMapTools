using System.Windows.Controls;
using BlokMap.ViewModels;

namespace BlokMap.Views
{
    /// <summary>Логика взаимодействия для TrackSelectorView.xaml</summary>
    public partial class TrackSelectorView : UserControl
    {
        public TrackSelectorView(TrackSelectorViewModel ViewModel)
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
