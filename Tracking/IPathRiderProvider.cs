using System;

namespace Tracking
{
    public interface IPathRiderProvider
    {
        IObservable<IPathRider> PathRider { get; }
    }
}
