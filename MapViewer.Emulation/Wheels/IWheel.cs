using System;

namespace MapViewer.Emulation.Wheels
{
    public interface IWheel
    {
        Double Speed { get; set; }
        IObservable<double> Milage { get; }
    }
}
