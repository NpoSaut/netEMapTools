using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Geographics;

namespace Tracking
{
    public interface IPathRider {
        EarthPoint PointAt(Double Offset);
    }

    /// <summary>Обеспечивает возможность навигации по треку</summary>
    public class TrackRider : IPathRider
    {
        public TrackRider(GpsTrack Track)
        {
            this.Track = Track;
            TrackSegments = SliceForSegments(Track);
        }

        public GpsTrack Track { get; private set; }

        private IList<TrackSegment> TrackSegments { get; set; }

        /// <summary>Нарезает трек на сегменты</summary>
        private static IList<TrackSegment> SliceForSegments(GpsTrack Track)
        {
            double offset = 0;
            var res = new List<TrackSegment>(Track.TrackPoints.Count - 1);
            for (int i = 1; i < Track.TrackPoints.Count; i++)
            {
                var segment = new TrackSegment(Track.TrackPoints[i - 1], Track.TrackPoints[i], offset);
                offset = segment.EndOffset;
                res.Add(segment);
            }
            return res;
        }

        private TrackSegment SegmentAt(Double Offset)
        {
            return TrackSegments.First(seg => seg.StartOffset <= Offset && Offset < seg.EndOffset);
        }

        public EarthPoint PointAt(Double Offset)
        {
            var segment = SegmentAt(Offset);
            var localOffset = Offset - segment.StartOffset;
            return segment.GetMiddlePoint(localOffset / segment.Length);
        }

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

            public EarthPoint StartPoint { get; private set; }
            public EarthPoint EndPoint { get; private set; }
            public Double Length { get; private set; }
            
            public Double StartOffset { get; private set; }
            public Double EndOffset { get; private set; }

            public EarthPoint GetMiddlePoint(Double Ratio) { return EarthPoint.MiddlePoint(StartPoint, EndPoint, Ratio); }
        }
    }
}
