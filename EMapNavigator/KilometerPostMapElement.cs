using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using MapVisualization.Elements;

namespace EMapNavigator
{
    public class KilometerPostMapElement : MapPointElement
    {
        public int Ordinate { get; private set; }

        public KilometerPostMapElement(GPost Post) : base(Post.Point) { this.Ordinate = Post.Ordinate; }

        protected override void DrawPointElement(DrawingContext dc)
        {
            var postLabelText = string.Format("{0}км", Ordinate / 1000);
            var postLabel = new FormattedText(postLabelText, CultureInfo.CurrentCulture,
                                         FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.DodgerBlue);

            dc.PushTransform(new TranslateTransform(-1, -26));

            dc.DrawRectangle(Brushes.White, new Pen(Brushes.DarkRed, 2), new Rect(0, 0, postLabel.Width + 4, postLabel.Height + 4));
            dc.DrawText(postLabel, new Point(2,2));
            dc.DrawLine(new Pen(Brushes.DarkRed, 2), new Point(0, 0), new Point(0, 26));

            dc.Pop();

            dc.DrawEllipse(Brushes.Aquamarine, new Pen(Brushes.SaddleBrown, 2), new Point(0, 0), 5, 5);
        }
    }
}