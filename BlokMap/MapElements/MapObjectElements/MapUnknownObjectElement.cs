using System;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;

namespace BlokMap.MapElements.MapObjectElements
{
    public class MapUnknownObjectElement : MapObjectElement
    {
        private const Double Size = 8;

        public MapUnknownObjectElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            double bw = 1;
            var accentPen = new Pen(Brushes.Red, 2);
            var backgroundPen = new Pen(Brushes.White, 2 + bw * 2);
            dc.DrawLine(backgroundPen, new Point(-0.5 * Size - bw, -0.5 * Size - bw), new Point(0.5 * Size + bw, 0.5 * Size + bw));
            dc.DrawLine(backgroundPen, new Point(0.5 * Size + bw, -0.5 * Size - bw), new Point(-0.5 * Size - bw, 0.5 * Size + bw));
            dc.DrawLine(accentPen, new Point(-0.5 * Size, -0.5 * Size), new Point(0.5 * Size, 0.5 * Size));
            dc.DrawLine(accentPen, new Point(0.5 * Size, -0.5 * Size), new Point(-0.5 * Size, 0.5 * Size));

            if (Zoom > 12)
                PrintDetails(dc, String.Format("{0} \"{1}\"", Target.Type, Target.Name));
        }
    }
}
