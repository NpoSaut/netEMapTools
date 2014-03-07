using System.Collections.Generic;
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
    }
}
