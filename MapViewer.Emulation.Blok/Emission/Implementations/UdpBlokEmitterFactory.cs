using System.Collections.Generic;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class UdpBlokEmitterFactory : BlokEmitterFactoryBase
    {
        public UdpBlokEmitterFactory() : base("Через UDP") { }
        protected override IBlokEmitter ProduceEmitter(ICollection<IEmissionOption> Options) { return new UdpBlokEmitter(); }
    }
}
