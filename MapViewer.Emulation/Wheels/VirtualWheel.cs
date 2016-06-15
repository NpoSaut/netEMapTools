using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MapViewer.Emulation.Wheels
{
    public class VirtualWheel : IWheel
    {
        private static readonly TimeSpan _timerInterval = TimeSpan.FromMilliseconds(100);

        private double _disstance;

        public VirtualWheel()
        {
            _disstance = 0;
            IConnectableObservable<double> milage = Observable.Interval(_timerInterval)
                                                              .Select(i => IncreaseDisstance())
                                                              .Distinct()
                                                              .Publish();
            Milage = milage;
            milage.Connect();
        }

        public double Speed { get; set; }
        public IObservable<double> Milage { get; private set; }

        private double IncreaseDisstance()
        {
            _disstance += Speed * (_timerInterval.TotalSeconds) / 3.6;
            return _disstance;
        }
    }
}
