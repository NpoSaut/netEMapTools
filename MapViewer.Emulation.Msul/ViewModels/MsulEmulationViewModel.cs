using System;
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

            ReactiveCommand<IDisposable> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => emulationSettings.Select(s => _emulationSource.EmulationSource(Parameters,
                                                                                        s.activeSection == 1
                                                                                            ? InitializationKind.HeadSection
                                                                                            : InitializationKind.TailSection)
                                                                       .EmitOn(s.left))
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
    }
}
