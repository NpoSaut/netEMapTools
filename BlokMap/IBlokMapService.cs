using System;
using System.Threading.Tasks;
using GMapElements;
using GMapElements.Entities;

namespace BlokMap
{
    public interface IBlokMapService
    {
        GMap CurrentMap { get; }
        event EventHandler<MapChangedEventArgs> CurrentMapChanged;
        Task LoadMap(string MapFileName);
    }

    public class MapChangedEventArgs : EventArgs
    {
        public MapChangedEventArgs(string FileName) { this.FileName = FileName; }
        public string FileName { get; private set; }
    }
}
