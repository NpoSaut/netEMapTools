using System;

namespace EMapNavigator.Emulation
{
    public interface IWheel
    {
        Double Milage { get; }
        Double Speed { get; set; }
        event EventHandler MilageChanged;
    }
}
