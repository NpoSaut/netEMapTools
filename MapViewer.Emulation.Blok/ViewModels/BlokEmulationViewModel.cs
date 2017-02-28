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
        private readonly ObservableAsPropertyHelper<bool> _canChangeDescriptor;
        private readonly ObservableAsPropertyHelper<bool> _canChangeEmissionMethod;
        private readonly IList<EmissionDescriptorViewModel> _emissionDescriptors;
        private readonly IBlokEmitterFactory[] _emitterFactories;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        private PositionPresenter _positionPresenter;
        private bool _reliability = true;
        private EmissionDescriptorViewModel _selectedDescriptor;
        private IBlokEmitterFactory _selectedEmitterFactory;

        public BlokEmulationViewModel(IBlokEmitterFactory[] EmitterFactories, BlokEmulationParameters EmulationParameters, IMappingService MappingService)
        {
            _emitterFactories = EmitterFactories;
            _emissionDescriptors = new[]
                                   {
                                       new EmissionDescriptorViewModel("Эмуляция", 0xb808),
                                       new EmissionDescriptorViewModel("ЭК-СНС", 0x4268)
                                   };

            ReactiveCommand<NavigationInformation> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => EmulationParameters.WhenAnyValue(x => x.Position,
                                                          x => x.Speed,
                                                          (position, speed) => new { position, speed })
                                            .CombineLatest(this.WhenAnyValue(x => x.Reliability),
                                                           (n, r) => new NavigationInformation(n.position, n.speed, r))
                                            .TakeUntil(_stop)
                                            .EmitThorough(SelectedEmitterFactory.CreatEmitter(SelectedDescriptor.Descriptor)));

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
            this.WhenAnyValue(x => x.SelectedEmitterFactory,
                              x => x.CanChangeEmissionMethod,
                              (emitter, canChange) => emitter.UsesDescriptor && canChange)
                .ToProperty(this, x => x.CanChangeDescriptor, out _canChangeDescriptor);

            SelectedEmitterFactory = EmitterFactories.First();
            SelectedDescriptor = _emissionDescriptors.First();

            _positionPresenter = new PositionPresenter(MappingService,
                                                       EmulationParameters.WhenAnyValue(x => x.Position));
        }

        public bool CanChangeDescriptor
        {
            get { return _canChangeDescriptor.Value; }
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

        public IList<EmissionDescriptorViewModel> EmissionDescriptors
        {
            get { return _emissionDescriptors; }
        }

        public EmissionDescriptorViewModel SelectedDescriptor
        {
            get { return _selectedDescriptor; }
            set { this.RaiseAndSetIfChanged(ref _selectedDescriptor, value); }
        }

        public bool CanChangeEmissionMethod
        {
            get { return _canChangeEmissionMethod.Value; }
        }
    }
}
