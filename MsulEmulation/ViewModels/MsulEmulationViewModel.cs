using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace MsulEmulation.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        public MsulEmulationViewModel(MsulEmulationParametersViewModel Parameters)
        {
            this.Parameters = Parameters;
            ReactiveCommand<Unit> run =
                ReactiveCommand.CreateAsyncObservable(_ => Observable.StartAsync(RunImpl).TakeUntil(_stop));

            _stop = ReactiveCommand.Create();

            run.Subscribe(_run => Debug.Print("started"));
            _stop.Subscribe(_run => Debug.Print("stopped"));

            Run = run;
            Stop = _stop;

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => running)
                .InvokeCommand(Run);
            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => !running)
                .InvokeCommand(Stop);
        }

        public MsulEmulationParametersViewModel Parameters { get; private set; }
        public ICommand Run { get; private set; }
        public ICommand Stop { get; private set; }

        public bool EmulationEnabled
        {
            get { return _emulationEnabled; }
            set { this.RaiseAndSetIfChanged(ref _emulationEnabled, value); }
        }

        private async Task RunImpl(CancellationToken CancellationToken)
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                Debug.Print("cyka ({0})", EmulationEnabled);
                await Task.Delay(100, CancellationToken);
            }
        }
    }
}
