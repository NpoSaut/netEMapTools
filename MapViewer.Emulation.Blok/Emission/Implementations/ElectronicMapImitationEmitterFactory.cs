using System.Collections.Generic;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class ElectronicMapImitationEmitterFactory : IBlokEmitterFactory
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;
        public ElectronicMapImitationEmitterFactory(ICanPortHandlerProvider CanPortHandlerProvider) { _canPortHandlerProvider = CanPortHandlerProvider; }
        public string Name => "IMIT_MM";
        public IBlokEmitter CreatEmitter(ICollection<IEmissionOption> Options) { return new ElectronicMapImitationEmitter(_canPortHandlerProvider); }
    }
}
