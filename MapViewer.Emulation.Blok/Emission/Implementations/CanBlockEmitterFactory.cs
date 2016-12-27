using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class CanBlockEmitterFactory : IBlokEmitterFactory
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;
        public CanBlockEmitterFactory(ICanPortHandlerProvider CanPortHandlerProvider) { _canPortHandlerProvider = CanPortHandlerProvider; }

        public string Name
        {
            get { return "Через CAN"; }
        }

        public IBlokEmitter CreatEmitter() { return new CanBlokEmitter(_canPortHandlerProvider); }
    }
}
