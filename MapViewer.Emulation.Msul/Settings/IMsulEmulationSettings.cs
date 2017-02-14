using System.ComponentModel;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Settings.Interfaces;

namespace MapViewer.Emulation.Msul.Settings
{
    public interface IMsulEmulationSettings : ISettings, INotifyPropertyChanged
    {
        EmissionLink LeftEmissionLink { get; }
        EmissionLink RightEmissionLink { get; }
    }
}
