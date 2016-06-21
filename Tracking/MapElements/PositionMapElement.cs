using System.Windows;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace Tracking.MapElements
{
    public class PositionMapElement : MapPointElement
    {
        public PositionMapElement(EarthPoint Position) : base(Position) { }

        /// <summary>Z-индекс элемента на карте</summary>
        /// <remarks>Меньшее значения индекса соответствуют нижним слоям на карте</remarks>
        protected override int ZIndex
        {
            get { return 20; }
        }

        protected override void DrawPointElement(DrawingContext dc) { dc.DrawEllipse(Brushes.Red, new Pen(Brushes.White, 2), new Point(), 5, 5); }
    }
}
