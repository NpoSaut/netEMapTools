using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.ViewModels.Options.Producing
{
    public interface IOptionViewModelsSetFactory
    {
        IList<IEmissionOption> CreateOptionsViewModels(Type EmitterFactoryType);
    }

    internal class OptionViewModelsSetFactory : IOptionViewModelsSetFactory
    {
        private readonly IEmissionOptionViewModelFactory[] _factories;
        public OptionViewModelsSetFactory(IEmissionOptionViewModelFactory[] Factories) { _factories = Factories; }

        public IList<IEmissionOption> CreateOptionsViewModels(Type EmitterFactoryType)
        {
            var requiredOptions = new HashSet<Type>(EmitterFactoryType.GetCustomAttributes<EmissionOptionAttribute>()
                                                                      .Select(a => a.OptionType));
            return _factories.Where(f => requiredOptions.Contains(f.OptionType))
                             .Select(f => f.CreateViewModel())
                             .ToList();
        }
    }
}
