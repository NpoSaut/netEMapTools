using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using GMapElements;
using MapVisualization.Elements;

namespace EMapNavigator.MapElements
{
    public class KilometerPostMapElement : MapPointElement
    {
        public int Ordinate { get; private set; }

        public KilometerPostMapElement(GPost Post) : base(Post.Point)
        {
            this.Ordinate = Post.Ordinate;
            SectionBrush = Brushes.Aquamarine;
        }

        private Brush mainBrush = Brushes.DarkSlateGray;
        public Brush SectionBrush { get; set; }

        protected override void DrawPointElement(DrawingContext dc)
        {
            var postLabelText = string.Format("{0}км", (double)Ordinate / 1000);
            var postLabel = new FormattedText(postLabelText, CultureInfo.CurrentCulture,
                                         FlowDirection.LeftToRight, new Typeface("Verdana"), 10, mainBrush);

            const int flagHeight = 22;

            dc.PushTransform(new TranslateTransform(0, -flagHeight));

            dc.DrawRectangle(Brushes.White, new Pen(mainBrush, 1), new Rect(-0.5, -0.5, Math.Round(postLabel.Width) + 5, Math.Round(postLabel.Height) + 2));
            dc.DrawText(postLabel, new Point(2,0));
            dc.DrawLine(new Pen(mainBrush, 2), new Point(0, 0), new Point(0, flagHeight));

            dc.Pop();

            dc.DrawEllipse(SectionBrush, new Pen(mainBrush, 1.5), new Point(0, 0), 5, 5);
        }
    }
}