using System;
using System.Reactive.Linq;
using MapViewer.Emulation.Wheels;

namespace SimpleMsul.Model
{
    internal class FakeWheel : IWheel
    {
        public double              Speed { get; set; } = 0;
        public event EventHandler  SpeedChanged;
        public IObservable<double> Milage => Observable.Return(0.0);

        public void Reset() { }
    }
}