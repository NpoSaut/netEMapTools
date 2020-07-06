using MapViewer.Emulation.Msul.ViewModels;

namespace MapViewer.Emulation.Msul.Encoding
{
    public interface IMsulMessageEncoder
    {
        byte[] GetMessage(MsulData Data, int Counter);
    }
}
