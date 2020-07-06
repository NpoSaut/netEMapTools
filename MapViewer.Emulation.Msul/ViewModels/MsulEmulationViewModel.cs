using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Msul.Emit;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class MsulEmulationViewModel : ReactiveObject
    {
        private readonly MsulEmulationSettingsViewModel _settings;
        private readonly ReactiveCommand<object> _stop;
        private bool _emulationEnabled;

        public MsulEmulationViewModel(
            MsulEmulationParametersViewModel Parameters, MsulEmulationSettingsViewModel Settings,
            IMsulEmitterFactory EmitterFactory)
        {
            _settings       = Settings;
            this.Parameters = Parameters;

            // ReSharper disable InvokeAsExtensionMethod
            var emulationSettings = Observable.CombineLatest(EmitterFactory.LeftEmitter,
                                                             EmitterFactory.RightEmitter,
                                                             Parameters.WhenAnyValue(x => x.ActiveSection),
                                                             (left, right, activeSection) =>
                                                                 new { left, right, activeSection });

            var run =
                ReactiveCommand.CreateAsyncObservable(
                    _ => emulationSettings.Select(s => new CompositeDisposable(Emit(s.activeSection == 1, s.left),
                                                                               Emit(s.activeSection == 5, s.right)))
                                          .Scan((CompositeDisposable) null,
                                                (old, current) =>
                                                {
                                                    old?.Dispose();
                                                    return current;
                                                })
                                          .TakeUntil(_stop));
            // ReSharper restore InvokeAsExtensionMethod

            _stop = ReactiveCommand.Create();

            run.Sample(_stop)
               .Subscribe(d => d.Dispose());

            Run  = run;
            Stop = _stop;

            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => running)
                .InvokeCommand(Run);
            this.WhenAnyValue(x => x.EmulationEnabled)
                .Where(running => !running)
                .InvokeCommand(Stop);
        }

        public MsulEmulationParametersViewModel Parameters { get; }
        public ICommand                         Run        { get; }
        public ICommand                         Stop       { get; }

        public bool EmulationEnabled
        {
            get => _emulationEnabled;
            set => this.RaiseAndSetIfChanged(ref _emulationEnabled, value);
        }
    }
}