using System;

namespace MapViewer.Emulation
{
    public interface INavigator
    {
        IObservable<NavigationInformation> Navigation { get; }
    }
}
