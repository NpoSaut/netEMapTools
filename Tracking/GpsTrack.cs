using System;
using System.Collections.Generic;
using System.Linq;
using Geographics;

namespace Tracking
{
    /// <summary>Трек</summary>
    public class GpsTrack
    {
        /// <summary>Создаёт пустой трек</summary>
        public GpsTrack() : this(new List<EarthPoint>()) { }

        /// <summary>Создаёт трек на основе указанного списка точек</summary>
        /// <param name="TrackPoints">Список точек</param>
        public GpsTrack(IList<EarthPoint> TrackPoints) { this.TrackPoints = TrackPoints; }

        /// <summary>Точки трека</summary>
        public IList<EarthPoint> TrackPoints { get; private set; }

        public Double Length
        {
            get
            {
                Double res = 0;
                for (int i = 1; i < TrackPoints.Count; i++) res += TrackPoints[i - 1].DistanceTo(TrackPoints[i]);
                return res;
            }
        }
    }
}
