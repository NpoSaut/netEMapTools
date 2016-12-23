using System;

namespace BlokMap
{
    public interface ITrackSource
    {
        IObservable<int> Track { get; }
    }
}
