using System.Reactive.Linq;
using System.Windows.Input;
using MsulEmulation.Emit;
using ReactiveUI;
using ReactiveUI.Legacy;
using ReactiveCommand = ReactiveUI.ReactiveCommand;
using ReactiveCommandMixins = ReactiveUI.ReactiveCommandMixins;

namespace MsulEmulation.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        public MsulEmulationViewModel(MsulEmulationParametersViewModel Parameters, MsulEmulationSource EmulationSource, IMsulEmitter Emitter)
        {
            this.Parameters = Parameters;
            var run =
                ReactiveCommand.CreateAsyncObservable(_ => EmulationSource.EmulationSource(Parameters)
                                                                          .EmitOn(Emitter)
                                                                          .TakeUntil(_stop));

            _stop = ReactiveCommand.Create();

            Run = run;
            Stop = _stop;

            ReactiveCommandMixins.InvokeCommand(this.WhenAnyValue(x => x.EmulationEnabled)
                                   .Where(running => running), Run);
            ReactiveCommandMixins.InvokeCommand(this.WhenAnyValue(x => x.EmulationEnabled)
                                   .Where(running => !running), Stop);
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
