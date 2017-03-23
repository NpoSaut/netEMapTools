using System.Collections.Generic;
using System.Linq;
using Geographics;

namespace Tracking
{
    /// <summary>Обеспечивает возможность навигации по треку</summary>
    public class TrackPathRider : IPathRider
    {
        private readonly IList<TrackSegment> _trackSegments;

        public TrackPathRider(GpsTrack Track) { _trackSegments = SliceForSegments(Track); }

        public EarthPoint PointAt(double Offset)
        {
            var segment = SegmentAt(Offset);
            if (segment == null)
                return _trackSegments.Select(s => s.EndPoint).DefaultIfEmpty(new EarthPoint()).Last();

            var localOffset = Offset - segment.StartOffset;
            return segment.GetMiddlePoint(localOffset / segment.Length);
        }

        /// <summary>Нарезает трек на сегменты</summary>
        private static IList<TrackSegment> SliceForSegments(GpsTrack Track)
        {
            if (Track.TrackPoints.Count == 0)
                return new TrackSegment[0];

            if (Track.TrackPoints.Count == 1)
                return new[] { new TrackSegment(Track.TrackPoints[0], Track.TrackPoints[0], 0) };

            double offset = 0;
            var res = new List<TrackSegment>(Track.TrackPoints.Count - 1);
            for (var i = 1; i < Track.TrackPoints.Count; i++)
            {
                var segment = new TrackSegment(Track.TrackPoints[i - 1], Track.TrackPoints[i], offset);
                offset = segment.EndOffset;
                res.Add(segment);
            }
            return res;
        }

        private TrackSegment SegmentAt(double Offset) { return _trackSegments.FirstOrDefault(seg => seg.StartOffset <= Offset && Offset < seg.EndOffset); }

        /// <summary>Сегмент трека - участок между двумя точками трека</summary>
        private class TrackSegment
        {
            public TrackSegment(EarthPoint StartPoint, EarthPoint EndPoint, double StartOffset)
            {
                this.EndPoint = EndPoint;
                this.StartPoint = StartPoint;
                Length = StartPoint.DistanceTo(EndPoint);
                this.StartOffset = StartOffset;
                EndOffset = StartOffset + Length;
            }

            public EarthPoint StartPoint { get; }
            public EarthPoint EndPoint { get; }
            public double Length { get; }

            public double StartOffset { get; }
            public double EndOffset { get; }

            public EarthPoint GetMiddlePoint(double Ratio) { return EarthPoint.MiddlePoint(StartPoint, EndPoint, Ratio); }
        }
    }
}
