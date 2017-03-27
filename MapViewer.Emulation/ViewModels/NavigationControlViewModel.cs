using System;
using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Wheels;
using ReactiveUI;

namespace MapViewer.Emulation.ViewModels
{
    public class NavigationControlViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<double> _disstance;

        private bool _reliability = true;
        private double _speed;

        public NavigationControlViewModel(IWheel Wheel, INavigatorConfig NavigatorConfig)
        {
            this.WhenAnyValue(x => x.Speed, speed => Wheel.Speed = speed)
                .Subscribe(s => Wheel.Speed = s);

            Wheel.Milage
                 .ToProperty(this, w => w.Disstance, out _disstance);

            this.WhenAnyValue(x => x.Reliability)
                .Subscribe(r => NavigatorConfig.Relability = r);

            var resetDisstanceCommand = ReactiveCommand.Create(Wheel.Milage.Select(m => m > 0).ObserveOnDispatcher());
            resetDisstanceCommand.Subscribe(_ => Wheel.Reset());
            ResetDisstance = resetDisstanceCommand;
        }

        public ICommand ResetDisstance { get; private set; }

        public double Speed
        {
            get { return _speed; }
            set { this.RaiseAndSetIfChanged(ref _speed, value); }
        }

        public double Disstance
        {
            get { return _disstance.Value; }
        }

        public bool Reliability
        {
            get { return _reliability; }
            set { this.RaiseAndSetIfChanged(ref _reliability, value); }
        }
    }
}
