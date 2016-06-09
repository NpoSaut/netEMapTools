using System.Collections.ObjectModel;
using Geographics;
using MapViewer.Mapping;
using MapVisualization.Elements;

namespace EMapNavigator.ViewModels
{
    public class MapViewModel : IMappingService
    {
        private readonly ObservableCollection<MapElement> _elements = new ObservableCollection<MapElement>();

        public MapViewModel()
        {
            MapCenter = new EarthPoint(new Degree(56.8779), new Degree(60.5905));
            ZoomLevel = 14;
        }

        public int ZoomLevel { get; set; }

        public EarthPoint MapCenter { get; private set; }

        public ObservableCollection<MapElement> Elements
        {
            get { return _elements; }
        }

        public void Display(MapElement Element) { Elements.Add(Element); }
        public void Remove(MapElement Element) { Elements.Remove(Element); }
    }
}
