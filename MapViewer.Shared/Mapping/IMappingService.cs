﻿using System;
using MapVisualization;
using MapVisualization.Elements;

namespace MapViewer.Mapping
{
    public interface IMappingService
    {
        void Display(MapElement Element);
        void Remove(MapElement Element);
        IObservable<MapMouseActionEventArgs> Clicks { get; }
    }
}
