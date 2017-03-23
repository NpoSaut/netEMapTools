using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Input;
using MapViewer.Emulation.Wheels;
using ReactiveUI;

namespace MapViewer.Emulation.ViewModels
{
    public class SpeedControlViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<double> _disstance;
        private double _speed;

        public SpeedControlViewModel(IWheel Wheel)
        {
            this.WhenAnyValue(x => x.Speed,
                              speed => Wheel.Speed = speed)
                .Subscribe(s => Wheel.Speed = s);

            Wheel.Milage
                 .ToProperty(this, w => w.Disstance, out _disstance);

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
    }
}
