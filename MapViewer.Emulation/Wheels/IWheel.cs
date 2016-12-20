using System;

namespace MapViewer.Emulation.Wheels
{
    public interface IWheel
    {
        Double Speed { get; set; }
        event EventHandler SpeedChanged;
        IObservable<double> Milage { get; }
        void Reset();
    }
}
