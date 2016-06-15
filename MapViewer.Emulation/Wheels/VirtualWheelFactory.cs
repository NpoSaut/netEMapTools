namespace MapViewer.Emulation.Wheels
{
    public class VirtualWheelFactory : IWheelFactory
    {
        public IWheel GetWheel() { return new VirtualWheel(); }
    }
}
