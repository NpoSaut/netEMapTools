using System.Linq;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using MapVisualization.Elements;

namespace EMapNavigator.MapElements.MapObjectElements
{
    public abstract class MapObjectElement : MapPointElement
    {
        public GObject Target { get; set; }
        public MapObjectElement(EarthPoint Position, GObject Target) : base(Position) { this.Target = Target; }

        protected static readonly SolidColorBrush TextBackgroundBrush = new SolidColorBrush(Colors.White) { Opacity = 0.7 };

        protected static void PrintStack(DrawingContext dc, params FormattedText[] labels)
        {
            dc.DrawRectangle(TextBackgroundBrush, null,
                             new Rect(-2, -1, labels.Max(l => l.Width) + 2, labels.Sum(l => l.Height + 1) + 2));
            double yOffset = 0;
            foreach (var label in labels)
            {
                dc.DrawText(label, new Point(0, yOffset));
                yOffset += label.Height + 1;
            }
        }

        protected string OrdinateString
        {
            get { return string.Format("{0:N0}", Target.Ordinate); }
        }
    }
}
