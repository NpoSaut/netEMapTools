using System;
using System.Collections.Generic;
using System.Windows.Media;
using Geographics;
using MapViewer.Mapping;
using MapVisualization.Elements;
using Tracking.MapElements;

namespace Tracking.Presenting
{
    public class TrackPresenter : ITrackPresenter
    {
        private readonly IMappingService _mappingService;
        public TrackPresenter(IMappingService MappingService) { _mappingService = MappingService; }

        public IDisposable DisplayTrack(IList<EarthPoint> Track)
        {
            var trackMapElement = new MapTrackElement(Track, new Pen(Brushes.Red, 2));

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
