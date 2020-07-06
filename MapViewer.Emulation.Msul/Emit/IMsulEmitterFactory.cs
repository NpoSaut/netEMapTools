namespace MapViewer.Emulation.Msul.Emit
{
    public interface IMsulEmitterFactory
    {
        IMsulEmitter Create(EmissionLink ForLink);
    }
}