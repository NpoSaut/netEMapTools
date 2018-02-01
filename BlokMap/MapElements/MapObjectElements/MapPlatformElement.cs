using System;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using GMapElements.Entities;

namespace BlokMap.MapElements.MapObjectElements
{
    public class MapPlatformElement : MapObjectElement
    {
        private const Double Width = 14;
        private const Double Height = 4;

        public MapPlatformElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            dc.DrawRectangle(Brushes.Chocolate, new Pen(Brushes.White, 2), new Rect(-0.5 * Width, -0.5 * Height, Width, Height));
            PrintDetails(dc, Target.Name);
        }
    }
}
