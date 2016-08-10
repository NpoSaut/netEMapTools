using Communications.Appi;

namespace MapViewer.Emulation.Blok.Can
{
    public interface IAppiDeviceFactory
    {
        AppiDev GetDevice();
    }
}