using System.Collections.Generic;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    [EmissionOption(typeof(IDescriptorEmissionOption))]
    public class CanBlokEmitterFactory : BlokEmitterFactoryBase
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;

        public CanBlokEmitterFactory(string Name, ICanPortHandlerProvider CanPortHandlerProvider) : base(Name)
        {
            _canPortHandlerProvider = CanPortHandlerProvider;
        }

        protected override IBlokEmitter ProduceEmitter(ICollection<IEmissionOption> Options)
        {
            return new CanBlokEmitter(_canPortHandlerProvider, Options.Of<IDescriptorEmissionOption>().EmissionDescriptor);
        }
    }
}
