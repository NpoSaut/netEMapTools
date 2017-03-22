using System.Collections.Generic;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public class UdpBlokEmitterFactory : IBlokEmitterFactory
    {
        public string Name
        {
            get { return "Через UDP"; }
        }

        public IBlokEmitter CreatEmitter(ICollection<IEmissionOption> Options) { return new UdpBlokEmitter(); }
    }
}
