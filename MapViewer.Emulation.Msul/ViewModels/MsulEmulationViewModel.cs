using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Encoding;
using MapViewer.Mapping;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly MsulEmulationSource _emulationSource;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        public MsulEmulationViewModel(MsulEmulationParametersViewModel Parameters, MsulEmulationSource EmulationSource,
                                      IMsulEmitterProvider EmitterProvider, IMappingService MappingService)
        {
            this.Parameters = Parameters;
            _emulationSource = EmulationSource;
            // ReSharper disable InvokeAsExtensionMethod
            ReactiveCommand<MsulMessage> run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => Observable.CombineLatest(EmitterProvider.LeftEmitter,
                                                  EmitterProvider.RightEmitter,
                                                  Parameters.WhenAnyValue(x => x.ActiveSection),
                                                  (left, right, active) => new { left, right, active })
                                   .SelectMany(x =>
                                               Observable.Merge(
                                                   _emulationSource.EmulationSource(Parameters,
                                                                                    x.active == 1
                                                                                        ? InitializationKind.HeadSection
                                                                                        : InitializationKind.TailSection)
                                                                   .EmitOn(x.left),
                                                   _emulationSource.EmulationSource(Parameters,
                                                                                    x.active == 5
                                                                                        ? InitializationKind.HeadSection
                                                                                        : InitializationKind.TailSection)
                                                                   .EmitOn(x.right)))
                                   .TakeUntil(_stop));
            // ReSharper restore InvokeAsExtensionMethod

            _stop = ReactiveCommand.Create();

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
