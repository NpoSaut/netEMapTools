namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class UdpBlokEmitterFactory : IBlokEmitterFactory
    {
        public string Name
        {
            get { return "Через UDP"; }
        }

        public bool UsesDescriptor
        {
            get { return false; }
        }

        public IBlokEmitter CreatEmitter(int Descriptor) { return new UdpBlokEmitter(); }
    }
}
