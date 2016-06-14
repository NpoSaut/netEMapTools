using System;

namespace Tracking.Presenting
{
    public interface ITrackPresenter
    {
        IDisposable DisplayTrack(GpsTrack Track);
    }
}
