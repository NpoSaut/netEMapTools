using System;
using System.Collections.Generic;
using Geographics;

namespace Tracking.Presenting
{
    public interface ITrackPresenter
    {
        IDisposable DisplayTrack(IList<EarthPoint> Track);
    }
}
