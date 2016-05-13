namespace EMapNavigator.Emulation
{
    public class VirtualWheelFactory : IWheelFactory
    {
        public IWheel GetWheel() { return new VirtualWheel(); }
    }
}
