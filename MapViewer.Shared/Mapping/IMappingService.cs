using System;
using System.Collections.Generic;
using Geographics;
using MapVisualization;
using MapVisualization.Elements;

namespace MapViewer.Mapping
{
    public interface IMappingService
    {
        IObservable<MapMouseActionEventArgs> Clicks { get; }
        void Display(MapElement Element);
        void Remove(MapElement Element);
        void Navigate(EarthPoint ToPoint);
    }

    public static class MappingServiceHelper
    {
        public static void Display(this IMappingService Service, IEnumerable<MapElement> Elements)
        {
            foreach (MapElement element in Elements)
                Service.Display(element);
        }

        public static void Remove(this IMappingService Service, IEnumerable<MapElement> Elements)
        {
            foreach (MapElement element in Elements)
                Service.Remove(element);
        }
    }
}
