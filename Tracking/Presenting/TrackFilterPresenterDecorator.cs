using System;
using System.Collections.Generic;
using System.Linq;
using Geographics;

namespace Tracking.Presenting
{
    public class TrackFilterPresenterDecorator : ITrackPresenter
    {
        private readonly ITrackPresenter _core;
        private readonly double _step;

        public TrackFilterPresenterDecorator(double Step, ITrackPresenter Core)
        {
            _core = Core;
            _step = Step;
        }

        public IDisposable DisplayTrack(IList<EarthPoint> Track)
        {
            List<EarthPoint> filtered = Filter(Track).ToList();
            return _core.DisplayTrack(filtered);
        }

        private IEnumerable<EarthPoint> Filter(IEnumerable<EarthPoint> Track)
        {
            EarthPoint? previousPoint = null;
            foreach (EarthPoint point in Track)
            {
                if (previousPoint == null || previousPoint.Value.DistanceTo(point) >= _step)
                {
                    previousPoint = point;
                    yield return point;
                }
            }
        }
    }
}
