using System.Windows.Controls;
using EMapNavigator.ViewModels;

namespace EMapNavigator.Views
{
    /// <summary>Логика взаимодействия для MappingToolbarView.xaml</summary>
    public partial class MappingToolbarView : UserControl
    {
        public MappingToolbarView(MappingToolbarViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
