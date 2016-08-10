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

        public BlokEmulationViewModel(IBlokEmitter Emitter, BlokEmulationParameters EmulationParameters, IMappingService MappingService)
        {
            _emitter = Emitter;

            ReactiveCommand<NavigationInformation> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => EmulationParameters.Changed
                                            .Select(x =>
                                                    new NavigationInformation(EmulationParameters.Position,
                                                                              EmulationParameters.Speed))
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

        public bool EmulationEnabled
        {
            get { return _emulationEnabled; }
            set { this.RaiseAndSetIfChanged(ref _emulationEnabled, value); }
        }
    }
}
