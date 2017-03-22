using System;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.ViewModels.Options.Producing
{
    public interface IEmissionOptionViewModelFactory
    {
        Type OptionType { get; }
        IEmissionOption CreateViewModel();
    }
}
