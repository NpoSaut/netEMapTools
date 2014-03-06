using System.Windows;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace EMapNavigator.MapElements
{
    public class MapMarkerElement : MapPointElement
    {
        /// <summary>Z-индекс элемента на карте</summary>
        /// <remarks>Меньшее значения индекса соответствуют нижним слоям на карте</remarks>
        protected override int ZIndex
        {
            get { return 20; }
        }

        public MapMarkerElement(EarthPoint Position) : base(Position) { }

        protected override void DrawPointElement(DrawingContext dc)
        {
            dc.DrawEllipse(Brushes.Red, new Pen(Brushes.White, 2), new Point(), 5, 5);
        }
    }
}