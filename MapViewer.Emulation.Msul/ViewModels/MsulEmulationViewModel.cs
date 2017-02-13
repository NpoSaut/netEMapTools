using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Mapping;
using ReactiveUI;
using Tracking;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly IMsulEmitter _emitter;
        private readonly MsulEmulationSource _emulationSource;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;
        private PositionPresenter _positionPresenter;

        public MsulEmulationViewModel(MsulEmulationParametersViewModel Parameters, MsulEmulationSource EmulationSource, IMsulEmitter Emitter,
                                      IMappingService MappingService)
        {
            this.Parameters = Parameters;
            _emulationSource = EmulationSource;
            _emitter = Emitter;
            ReactiveCommand<MsulMessage> run =
                ReactiveCommand.CreateAsyncObservable(_ => _emulationSource.EmulationSource(Parameters)
                                                                           .EmitOn(_emitter)
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
                                                       Parameters.WhenAnyValue(x => x.Position));
        }

        public MsulEmulationParametersViewModel Parameters { get; private set; }
        public ICommand Run { get; private set; }
        public ICommand Stop { get; private set; }

        public bool EmulationEnabled
        {
            get { return _emulationEnabled; }
            set { this.RaiseAndSetIfChanged(ref _emulationEnabled, value); }
        }
    }
}
