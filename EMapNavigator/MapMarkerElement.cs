using System.Windows;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace EMapNavigator
{
    public class MapMarkerElement : MapPointElement
    {
        public MapMarkerElement(EarthPoint Position) : base(Position) { }

        protected override void DrawPointElement(DrawingContext dc)
        {
            dc.DrawEllipse(Brushes.Red, new Pen(Brushes.White, 2), new Point(), 5, 5);
        }
    }
}