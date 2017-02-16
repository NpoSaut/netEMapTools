using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Encoding;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly MsulEmulationSource _emulationSource;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        public MsulEmulationViewModel(MsulEmulationParametersViewModel Parameters, MsulEmulationSource EmulationSource,
                                      IMsulEmitterProvider EmitterProvider)
        {
            this.Parameters = Parameters;
            _emulationSource = EmulationSource;

            // ReSharper disable InvokeAsExtensionMethod
            var emulationSettings = Observable.CombineLatest(EmitterProvider.LeftEmitter,
                                                             EmitterProvider.RightEmitter,
                                                             Parameters.WhenAnyValue(x => x.ActiveSection),
                                                             (left, right, activeSection) => new { left, right, activeSection });

            ReactiveCommand<CompositeDisposable> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => emulationSettings.Select(s => new CompositeDisposable(Emit(s.activeSection == 1, s.left),
                                                                               Emit(s.activeSection == 5, s.right)))
                                          .Scan((old, current) =>
                                                {
                                                    old.Dispose();
                                                    return current;
                                                })
                                          .TakeUntil(_stop));
            // ReSharper restore InvokeAsExtensionMethod

            _stop = ReactiveCommand.Create();

            run.Sample(_stop)
               .Subscribe(d => d.Dispose());

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

        private IDisposable Emit(bool Active, IMsulEmitter Emitter)
        {
            return _emulationSource.EmulationSource(Parameters,
                                                    Active
                                                        ? InitializationKind.HeadSection
                                                        : InitializationKind.TailSection)
                                   .EmitOn(Emitter);
        }
    }
}
