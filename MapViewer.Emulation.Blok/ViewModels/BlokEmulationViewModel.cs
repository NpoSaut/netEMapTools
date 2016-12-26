using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Blok.Emission;
using MapViewer.Mapping;
using ReactiveUI;
using Tracking;

namespace MapViewer.Emulation.Blok.ViewModels
{
    public class BlokEmulationViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _canChangeEmissionMethod;
        private readonly IBlokEmitterFactory[] _emitterFactories;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        private PositionPresenter _positionPresenter;
        private bool _reliability = true;
        private IBlokEmitterFactory _selectedEmitterFactory;

        public BlokEmulationViewModel(IBlokEmitterFactory[] EmitterFactories, BlokEmulationParameters EmulationParameters, IMappingService MappingService)
        {
            _emitterFactories = EmitterFactories;

            ReactiveCommand<NavigationInformation> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => EmulationParameters.WhenAnyValue(x => x.Position,
                                                          x => x.Speed,
                                                          (position, speed) => new { position, speed })
                                            .CombineLatest(this.WhenAnyValue(x => x.Reliability),
                                                           (n, r) => new NavigationInformation(n.position, n.speed, r))
                                            .TakeUntil(_stop)
                                            .EmitThorough(SelectedEmitterFactory.CreatEmitter()));

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

            SelectedEmitterFactory = EmitterFactories.First();

            _positionPresenter = new PositionPresenter(MappingService,
                                                       EmulationParameters.WhenAnyValue(x => x.Position));
        }

        public ICommand Run { get; private set; }
        public ICommand Stop { get; private set; }

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
