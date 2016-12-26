using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanBlockEmitterFactory : IBlokEmitterFactory
    {
        private readonly IAppiDeviceFactory _appiDeviceFactory;
        public CanBlockEmitterFactory(IAppiDeviceFactory AppiDeviceFactory) { _appiDeviceFactory = AppiDeviceFactory; }

        public string Name
        {
            get { return "Через CAN"; }
        }

        public IBlokEmitter CreatEmitter() { return new CanBlokEmitter(_appiDeviceFactory); }
    }
}
