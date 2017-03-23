using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;

namespace BlokMap.MapElements.MapObjectElements
{
    public class MapTrafficLightElement : MapObjectElement
    {
        private const int BodyOffset = 4;
        private const int LightRadius = 2;
        private const double BodyWidth = LightRadius * 2 + 2;
        private static readonly Brush[] _lights = { Brushes.Red, Brushes.Yellow, Brushes.GreenYellow };
        private static readonly double _bodyHeight = _lights.Length * BodyWidth;

        public MapTrafficLightElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            dc.DrawLine(new Pen(Brushes.Black, 2), new Point(), new Point(0, -BodyOffset));
            dc.PushTransform(new TranslateTransform(0, -0.5 * _bodyHeight - BodyOffset));
            dc.DrawRectangle(Brushes.Black, null, new Rect(-0.5 * BodyWidth, -0.5 * _bodyHeight, BodyWidth, _bodyHeight));

            for (var i = 0; i < _lights.Length; i++)
                dc.DrawEllipse(_lights[i], null, new Point(0, (2 + LightRadius + 1) * (i - 1)), LightRadius, LightRadius);

            dc.Pop();

            if (Zoom > 12 || IsMouseOver)
                PrintDetails(dc, Target.Name);
        }
    }
}
