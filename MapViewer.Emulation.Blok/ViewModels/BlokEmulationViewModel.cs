using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Emulation.Blok.Emission.Options;
using MapViewer.Emulation.Blok.ViewModels.Options.Producing;
using ReactiveUI;

namespace MapViewer.Emulation.Blok.ViewModels
{
    public class BlokEmulationViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _canChangeEmissionMethod;
        private readonly ObservableAsPropertyHelper<IList<IEmissionOption>> _emissionOptions;
        private readonly IBlokEmitterFactory[] _emitterFactories;
        private bool _emulationEnabled;

        private IBlokEmitterFactory _selectedEmitterFactory;

        public BlokEmulationViewModel(IBlokEmitterFactory[] EmitterFactories, INavigator Navigator,
                                      IOptionViewModelsSetFactory OptionsFactory)
        {
            _emitterFactories = EmitterFactories;
            SelectedEmitterFactory = EmitterFactories.First();

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Select(enabled => enabled
                                       ? SelectedEmitterFactory.CreatEmitter(EmissionOptions, Navigator.Navigation)
                                       : Disposable.Empty)
                .DisposePrevious()
                .Subscribe();

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Select(e => !e)
                .ToProperty(this, x => x.CanChangeEmissionMethod, out _canChangeEmissionMethod, true);

            this.WhenAnyValue(x => x.SelectedEmitterFactory)
                .Select(fac => OptionsFactory.CreateOptionsViewModels(fac.GetType()))
                .ToProperty(this, x => x.EmissionOptions, out _emissionOptions);
        }

        public IList<IEmissionOption> EmissionOptions
        {
            get { return _emissionOptions.Value; }
        }

        public IBlokEmitterFactory SelectedEmitterFactory
        {
            get { return _selectedEmitterFactory; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmitterFactory, value); }
        }

        public bool EmulationEnabled
        {
            get { return _emulationEnabled; }
            set { this.RaiseAndSetIfChanged(ref _emulationEnabled, value); }
        }

        public IList<IBlokEmitterFactory> EmitterFactories
        {
            get { return _emitterFactories; }
        }

        public bool CanChangeEmissionMethod
        {
            get { return _canChangeEmissionMethod.Value; }
        }
    }
}
