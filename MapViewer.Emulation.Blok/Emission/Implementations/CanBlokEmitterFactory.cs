using System.Collections.Generic;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    [EmissionOption(typeof (IDescriptorEmissionOption))]
    public class CanBlokEmitterFactory : IBlokEmitterFactory
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;

        public CanBlokEmitterFactory(string Name, ICanPortHandlerProvider CanPortHandlerProvider)
        {
            _canPortHandlerProvider = CanPortHandlerProvider;
            this.Name = Name;
        }

        public string Name { get; private set; }

        public IBlokEmitter CreatEmitter(ICollection<IEmissionOption> Options)
        {
            return new CanBlokEmitter(_canPortHandlerProvider, Options.Of<IDescriptorEmissionOption>().EmissionDescriptor);
        }
    }
}
