using System;
using System.Reactive.Linq;
using Geographics;
using MapViewer.Mapping;
using Tracking.MapElements;

namespace Tracking
{
    public class PositionPresenter
    {
        private readonly IMappingService _mappingService;
        private PositionMapElement _currentElement;

        public PositionPresenter(IMappingService MappingService, IObservable<EarthPoint> Position)
        {
            _mappingService = MappingService;
            Position.ObserveOnDispatcher()
                    .Subscribe(RefreshPosition);
        }

        private void RefreshPosition(EarthPoint Position)
        {
            _mappingService.Remove(_currentElement);
            _currentElement = new PositionMapElement(Position);
            _mappingService.Display(_currentElement);
        }
    }
}
