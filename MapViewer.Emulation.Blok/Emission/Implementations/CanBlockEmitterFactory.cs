using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanBlockEmitterFactory : IBlokEmitterFactory
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;

        public CanBlockEmitterFactory(string Name, ICanPortHandlerProvider CanPortHandlerProvider)
        {
            _canPortHandlerProvider = CanPortHandlerProvider;
            this.Name = Name;
        }

        public string Name { get; private set; }

        public IBlokEmitter CreatEmitter() { return new CanBlokEmitter(_canPortHandlerProvider); }
    }
}
