using System.Windows;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace BlokMap.MapElements
{
    public class SearchHighlightMapElement : MapPointElement
    {
        private const int Radius = 17;
        private static readonly Brush _fill = new SolidColorBrush(Color.FromArgb(140, 0, 132, 225));
        private static readonly Pen _stroke = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 132, 225)), 0.6);

        public SearchHighlightMapElement(EarthPoint Position) : base(Position) { }

        protected override int ZIndex
        {
            get { return -1; }
        }

        protected override void DrawPointElement(DrawingContext dc, int Zoom) { dc.DrawEllipse(_fill, _stroke, new Point(), Radius, Radius); }
    }
}
