using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Mapping;
using ReactiveUI;
using Tracking;

namespace MapViewer.Emulation.Blok.ViewModels
{
    public class BlokEmulationViewModel : ReactiveObject
    {
        private readonly IBlokEmitter _emitter;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        private PositionPresenter _positionPresenter;
        private bool _reliability = true;

        public BlokEmulationViewModel(IBlokEmitter Emitter, BlokEmulationParameters EmulationParameters, IMappingService MappingService)
        {
            _emitter = Emitter;

            ReactiveCommand<NavigationInformation> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => EmulationParameters.WhenAnyValue(x => x.Position,
                                                          x => x.Speed,
                                                          (position, speed) => new { position, speed })
                                            .CombineLatest(this.WhenAnyValue(x => x.Reliability),
                                                           (n, r) => new NavigationInformation(n.position, n.speed, r))
                                            .EmitThorough(Emitter)
                                            .TakeUntil(_stop));

            _stop = ReactiveCommand.Create();

            Run = run;
            Stop = _stop;

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => running)
                .InvokeCommand(Run);
            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => !running)
                .InvokeCommand(Stop);

            _positionPresenter = new PositionPresenter(MappingService,
                                                       EmulationParameters.WhenAnyValue(x => x.Position));
        }

        public ICommand Run { get; private set; }
        public ICommand Stop { get; private set; }

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
    }
}
