namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class UdpBlokEmitterFactory : IBlokEmitterFactory
    {
        public string Name
        {
            get { return "Через UDP"; }
        }

        public IBlokEmitter CreatEmitter() { return new UdpBlokEmitter(); }
    }
}
