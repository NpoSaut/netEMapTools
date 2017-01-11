using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanOverUdpBlokEmitterFactory : IBlokEmitterFactory
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;

        public CanOverUdpBlokEmitterFactory(string Name, ICanPortHandlerProvider CanPortHandlerProvider)
        {
            this.Name = Name;
            _canPortHandlerProvider = CanPortHandlerProvider;
        }

        public string Name { get; private set; }
        public IBlokEmitter CreatEmitter() { return new CanOverUdpBlokEmitter(_canPortHandlerProvider); }
    }
}
