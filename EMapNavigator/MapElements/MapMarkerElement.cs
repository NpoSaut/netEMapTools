using System;
using System.Windows;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace EMapNavigator.MapElements
{
    public class MapMarkerElement : MapPointElement
    {
        private const double R = 7;
        private const double H = 14;
        private const double Angle = Math.PI / 6;

        public MapMarkerElement(EarthPoint Position) : base(Position) { }

        protected override int ZIndex
        {
            get { return 5; }
        }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            var pathSegments = new PathSegment[]
                               {
                                   new LineSegment(new Point(R * Math.Sin(+Angle), -H + R * Math.Cos(+Angle)), true),
                                   new ArcSegment(new Point(R * Math.Sin(-Angle), -H + R * Math.Cos(-Angle)),
                                                  new Size(R, R), 2 * (Math.PI - Angle),
                                                  true, SweepDirection.Counterclockwise, true),
                                   new LineSegment(new Point(0, 0), true)
                               };

            var markerGeometry = new PathGeometry(new[]
                                                  {
                                                      new PathFigure(new Point(), pathSegments, true)
                                                  });
            var shadowGeometry = markerGeometry.Clone();
            var tg = new TransformGroup();
            tg.Children.Add(new SkewTransform(-40, 0));
            tg.Children.Add(new TranslateTransform(0, 1.4));
            shadowGeometry.Transform = tg;

            var shadowGradientBrush = new LinearGradientBrush(Colors.Transparent, Color.FromArgb(180, 0, 0, 0),
                                                              new Point(0, 0.2), new Point(0, 1));

            dc.DrawGeometry(shadowGradientBrush, null, shadowGeometry);
            dc.DrawGeometry(Brushes.White, new Pen(Brushes.Crimson, 1.4), markerGeometry);

            dc.DrawEllipse(Brushes.Crimson, null, new Point(0, -H), 0.4 * R, 0.4 * R);
        }
    }
}
