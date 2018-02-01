using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using GMapElements.Entities;

namespace BlokMap.MapElements.MapObjectElements
{
    public class MapDangerousPlaceObjectElement : MapObjectElement
    {
        private const double LegLength = 8;
        private const double Radius = 8;

        public MapDangerousPlaceObjectElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            dc.DrawLine(new Pen(Brushes.Black, 2), new Point(0, 0), new Point(0, -LegLength));
            dc.PushTransform(new TranslateTransform(0, -Radius - LegLength));
            dc.DrawEllipse(Brushes.White, new Pen(Brushes.Red, 1.5), new Point(0, 0), Radius, Radius);
            var label = new FormattedText((Target.SpeedRestriction - 3).ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                          new Typeface("Verdana"), 9, Brushes.Black);
            dc.DrawText(label, new Point(-label.Width * 0.5, -label.Height * 0.5));
            dc.Pop();

            PrintDetails(dc, Target.Name);
        }
    }
}
