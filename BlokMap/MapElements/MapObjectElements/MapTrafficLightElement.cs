using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using GMapElements.Entities;

namespace BlokMap.MapElements.MapObjectElements
{
    public class MapTrafficLightElement : MapObjectElement
    {
        protected const int BodyOffset = 4;
        protected const int LightRadius = 2;
        protected const double BodyWidth = LightRadius * 2 + 2;
        protected static readonly Brush[] Lights = { Brushes.Red, Brushes.Yellow, Brushes.GreenYellow };
        protected static readonly double BodyHeight = Lights.Length * BodyWidth;

        public MapTrafficLightElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            dc.DrawLine(new Pen(Brushes.Black, 2), new Point(), new Point(0, -BodyOffset));
            dc.PushTransform(new TranslateTransform(0, - 0.5 * BodyHeight - BodyOffset));
            dc.DrawRectangle(Brushes.Black, null, new Rect(-0.5 * BodyWidth, -0.5 * BodyHeight, BodyWidth, BodyHeight));

            for (int i = 0; i < Lights.Length; i++)
                dc.DrawEllipse(Lights[i], null, new Point(0, (2 + LightRadius + 1) * (i - 1)), LightRadius, LightRadius);

            dc.Pop();

            if (Zoom > 12 || IsMouseOver)
                PrintDetails(dc, Target.Name);
        }
    }
}
