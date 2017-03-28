using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using MsulEmulation.Emit;
using ReactiveUI;

namespace MsulEmulation.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> _stop;
        private bool _emulationEnabled;

        public MsulEmulationViewModel(MsulEmulationParametersViewModel Parameters, MsulEmulationSource EmulationSource, IMsulEmitter Emitter)
        {
            this.Parameters = Parameters;
            Run = ReactiveCommand.CreateFromObservable(
                () => EmulationSource.EmulationSource(Parameters)
                                     .EmitOn(Emitter)
                                     .TakeUntil(_stop));

            _stop = ReactiveCommand.Create(() => { });
            Stop = _stop;

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => running)
                .InvokeCommand(Run);
            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => !running)
                .InvokeCommand(Stop);
        }

        public MsulEmulationParametersViewModel Parameters { get; private set; }
        public ICommand Run { get; }
        public ICommand Stop { get; }

        public bool EmulationEnabled
        {
            get { return _emulationEnabled; }
            set { this.RaiseAndSetIfChanged(ref _emulationEnabled, value); }
        }
    }
}
