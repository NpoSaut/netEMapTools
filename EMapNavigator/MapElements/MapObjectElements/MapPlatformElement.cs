using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;

namespace EMapNavigator.MapElements.MapObjectElements
{
    public class MapPlatformElement : MapObjectElement
    {
        private const Double Width = 14;
        private const Double Height = 4;

        public MapPlatformElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.Chocolate, new Pen(Brushes.White, 2), new Rect(-0.5 * Width, -0.5 * Height, Width, Height));
            
            var nameLabel = new FormattedText(Target.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.DarkRed);
            var ordinateLabel = new FormattedText(OrdinateString, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.DarkRed);

            dc.PushTransform(new TranslateTransform(-0.5 * Math.Max(nameLabel.Width, ordinateLabel.Width), 0.5 * Height + 3));
            PrintStack(dc, nameLabel, ordinateLabel);
            dc.Pop();
        }
    }
}