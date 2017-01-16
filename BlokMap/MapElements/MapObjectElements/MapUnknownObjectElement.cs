using System;
using System.Globalization;
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

            var nameLabel = new FormattedText(String.Format("{0} \"{1}\"", Target.Type, Target.Name), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                              new Typeface("Verdana"), 10, Brushes.Black);
            var ordinateLabel = new FormattedText(OrdinateString, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 8,
                                                  Brushes.Blue);

            dc.PushTransform(new TranslateTransform(-0.5 * Math.Max(nameLabel.Width, ordinateLabel.Width), 0.5 * Size + 3));
            PrintStack(dc, nameLabel, ordinateLabel);
            dc.Pop();
        }
    }
}
