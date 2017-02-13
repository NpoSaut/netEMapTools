using MapViewer.Emulation.Msul.ViewModels;

namespace MapViewer.Emulation.Msul.Encoding
{
    public interface IMsulMessageEncoder
    {
        MsulMessage GetMessage(MsulEmulationParametersViewModel ViewModel);
    }
}
