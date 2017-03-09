using System;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.ViewModels.Options.Producing
{
    internal class EmissionOptionViewModelFactory<TOption> : IEmissionOptionViewModelFactory
        where TOption : IEmissionOption
    {
        private readonly Func<IEmissionOption> _factory;

        public EmissionOptionViewModelFactory(Func<IEmissionOption> Factory) { _factory = Factory; }

        public Type OptionType
        {
            get { return typeof (TOption); }
        }

        public IEmissionOption CreateViewModel() { return _factory(); }
    }
}
