using MapViewer.Emulation.Msul.ViewModels;
using ReactiveUI;

namespace SimpleMsul.ViewModels
{
    public class EmulatorViewModel : ReactiveObject
    {
        public EmulatorViewModel(MsulEmulationParametersViewModel Parameters)
        {
            this.Parameters = Parameters;
        }

        public MsulEmulationParametersViewModel Parameters { get; }
    }
}