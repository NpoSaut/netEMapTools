using System;
using MapViewer.Emulation.Wheels;
using MapViewer.ViewModels;
using ReactiveUI;

namespace MapViewer.Emulation.ViewModels
{
    public class SpeedControlViewModel : ReactiveObject, ISpeedControlViewModel
    {
        private readonly ObservableAsPropertyHelper<double> _disstance;
        private readonly IWheel _wheel;
        private double _speed;

        public SpeedControlViewModel(IWheel Wheel)
        {
            _wheel = Wheel;

            this.WhenAnyValue(x => x.Speed,
                              speed => _wheel.Speed = speed)
                .Subscribe(s => _wheel.Speed = s);

            _wheel.Milage
                  .ToProperty(this, w => w.Disstance, out _disstance);
        }

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
