using System.Windows.Controls;
using Tracking.ViewModels;

namespace Tracking.Views
{
    /// <summary>Логика взаимодействия для TrackingControlView.xaml</summary>
    public partial class TrackingControlView : UserControl
    {
        public TrackingControlView(TrackingControlViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
