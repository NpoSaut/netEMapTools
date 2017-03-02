using System;
using GMapElements;

namespace BlokMap
{
    public interface IBlokMapService
    {
        GMap CurrentMap { get; }
        void SwitchMap(GMap Map);
        event EventHandler CurrentMapChanged;
    }
}