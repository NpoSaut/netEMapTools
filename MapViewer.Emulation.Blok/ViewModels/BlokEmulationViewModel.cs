using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
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
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        private bool _reliability = true;
        private IBlokEmitterFactory _selectedEmitterFactory;

        public BlokEmulationViewModel(IBlokEmitterFactory[] EmitterFactories, INavigator Navigator,
                                      IOptionViewModelsSetFactory OptionsFactory)
        {
            _emitterFactories = EmitterFactories;

            var run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => Navigator.Navigation
                                  .TakeUntil(_stop)
                                  .EmitThorough(SelectedEmitterFactory.CreatEmitter(EmissionOptions)));

            _stop = ReactiveCommand.Create();

            Run = run;
            Stop = _stop;

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => running)
                .InvokeCommand(Run);
            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => !running)
                .InvokeCommand(Stop);

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Select(e => !e)
                .ToProperty(this, x => x.CanChangeEmissionMethod, out _canChangeEmissionMethod, true);

            this.WhenAnyValue(x => x.SelectedEmitterFactory)
                .Select(fac => OptionsFactory.CreateOptionsViewModels(fac.GetType()))
                .ToProperty(this, x => x.EmissionOptions, out _emissionOptions);

            SelectedEmitterFactory = EmitterFactories.First();
        }

        public IList<IEmissionOption> EmissionOptions
        {
            get { return _emissionOptions.Value; }
        }

        public ICommand Run { get; }
        public ICommand Stop { get; }

        public IBlokEmitterFactory SelectedEmitterFactory
        {
            get { return _selectedEmitterFactory; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmitterFactory, value); }
        }

        public bool Reliability
        {
            get { return _reliability; }
            set { this.RaiseAndSetIfChanged(ref _reliability, value); }
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
