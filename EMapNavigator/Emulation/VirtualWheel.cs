using System;
using System.Timers;

namespace EMapNavigator.Emulation
{
    public class VirtualWheel : IWheel
    {
        private readonly Timer _timer;

        public VirtualWheel()
        {
            _timer = new Timer(100);
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        public double Milage { get; private set; }

        public double Speed { get; set; }
        public event EventHandler MilageChanged;

        private void TimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            Milage += Speed * (_timer.Interval / 1000.0) / 3.6;
            OnMilageChanged();
        }

        protected virtual void OnMilageChanged()
        {
            EventHandler handler = MilageChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
