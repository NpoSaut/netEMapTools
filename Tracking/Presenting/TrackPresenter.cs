using System;
using System.Windows.Media;
using MapViewer.Mapping;
using MapVisualization.Elements;
using Tracking.MapElements;

namespace Tracking.Presenting
{
    public class TrackPresenter : ITrackPresenter
    {
        private readonly IMappingService _mappingService;
        public TrackPresenter(IMappingService MappingService) { _mappingService = MappingService; }

        public IDisposable DisplayTrack(GpsTrack Track)
        {
            var trackMapElement = new MapTrackElement(Track.TrackPoints, new Pen(Brushes.Red, 2));

            _mappingService.Display(trackMapElement);
            return new TrackDisplaying(trackMapElement, _mappingService);
        }

        private class TrackDisplaying : IDisposable
        {
            private readonly IMappingService _mappingService;
            private readonly MapElement _trackMapElement;

            public TrackDisplaying(MapElement TrackMapElement, IMappingService MappingService)
            {
                _trackMapElement = TrackMapElement;
                _mappingService = MappingService;
            }

            public void Dispose() { _mappingService.Remove(_trackMapElement); }
        }
    }
}
