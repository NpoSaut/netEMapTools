using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Geographics;

namespace Tracking.Presenting
{
    public class SegmentationTrackPresenterDecorator : ITrackPresenter
    {
        private readonly ITrackPresenter _core;
        private readonly int _segmentLength;

        public SegmentationTrackPresenterDecorator(int SegmentLength, ITrackPresenter Core)
        {
            _core = Core;
            _segmentLength = SegmentLength;
        }

        public IDisposable DisplayTrack(IList<EarthPoint> Track) { return new CompositeDisposable(Segment(Track).Select(_core.DisplayTrack)); }

        private IEnumerable<IList<EarthPoint>> Segment(IEnumerable<EarthPoint> Track)
        {
            var segment = new List<EarthPoint>(_segmentLength);
            foreach (EarthPoint point in Track)
            {
                segment.Add(point);
                if (segment.Count == _segmentLength)
                {
                    yield return segment;
                    segment = new List<EarthPoint>(_segmentLength) { point };
                }
            }
            if (segment.Count > 1)
                yield return segment;
        }
    }
}
