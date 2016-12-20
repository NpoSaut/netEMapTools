using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MapViewer.Emulation.Wheels
{
    public class VirtualWheel : IWheel
    {
        private static readonly TimeSpan _timerInterval = TimeSpan.FromMilliseconds(100);

        private double _disstance;

        private bool _resetRequested;
        private double _speed;

        public VirtualWheel()
        {
            _disstance = 0;
            IConnectableObservable<double> milage = Observable.Interval(_timerInterval)
                                                              .Select(i => IncreaseDisstance())
                                                              .DistinctUntilChanged()
                                                              .Publish();
            Milage = milage;
            milage.Connect();
        }

        public event EventHandler SpeedChanged;

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    if (SpeedChanged != null) SpeedChanged(this, new EventArgs());
                }
            }
        }

        public IObservable<double> Milage { get; private set; }

        public void Reset() { _resetRequested = true; }

        private double IncreaseDisstance()
        {
            _disstance += Speed * (_timerInterval.TotalSeconds) / 3.6;
            if (_resetRequested)
            {
                _disstance = 0;
                _resetRequested = false;
            }
            return _disstance;
        }
    }
}
