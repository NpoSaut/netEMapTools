using System.ComponentModel;

namespace MapViewer.ViewModels
{
    public interface ISpeedControlViewModel : INotifyPropertyChanged
    {
        double Speed { get; set; }
        double Disstance { get; }
    }
}
