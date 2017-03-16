using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GMapElements;
using MapVisualization.Elements;

namespace BlokMap.MapElements
{
    [ZoomRestriction(0)]
    public class KilometerPostMapElement : MapPointElement
    {
        private readonly Brush mainBrush = Brushes.DarkSlateGray;

        public KilometerPostMapElement(GPost Post) : base(Post.Point)
        {
            this.Post = Post;
            SectionBrush = Brushes.Aquamarine;
        }

        public GPost Post { get; set; }

        public Brush SectionBrush { get; set; }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            if (Zoom > 12 || IsMouseOver)
            {
                string postLabelText = string.Format("{0}км", (double)Post.Ordinate / 1000);
                var postLabel = new FormattedText(postLabelText, CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight, new Typeface("Verdana"), 10, mainBrush);

                const int flagHeight = 22;
                dc.PushTransform(new TranslateTransform(0, -flagHeight));

                dc.DrawRectangle(Brushes.White, new Pen(mainBrush, 1), new Rect(-0.5, -0.5, Math.Round(postLabel.Width) + 5, Math.Round(postLabel.Height) + 2));
                dc.DrawText(postLabel, new Point(2, 0));
                dc.DrawLine(new Pen(mainBrush, 2), new Point(0, 0), new Point(0, flagHeight));

                dc.Pop();
            }

            if (Zoom > 8)
                dc.DrawEllipse(SectionBrush, new Pen(mainBrush, 1.5), new Point(0, 0), 5, 5);
            else
            {
                if (Post.Ordinate % (1000 * Math.Pow(2, 9 - Zoom)) == 0)
                    dc.DrawRectangle(SectionBrush, null, new Rect(-2, -2, 4, 4));
            }

            if (IsMouseOver)
            {
                dc.PushTransform(new TranslateTransform(3, 10));
                PrintStack(dc,
                           new FormattedText(Post.Direction == OrdinateDirection.Increasing ? "Возрастает по неч." : "Убывает по неч.",
                                             CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.DarkBlue),
                           new FormattedText(String.Format("Пути: {0}", string.Join(", ", Post.Tracks.Select(TrackName))),
                                             CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.DarkOliveGreen));
                dc.Pop();
            }
        }

        private string TrackName(GTrack Track)
        {
            if (Track.Number <= 15)
                return string.Format("{0}П", Track.Number);
            return string.Format("{0}Н", Track.Number - 15);
        }

        protected double PrintTrack(DrawingContext dc, GTrack Track)
        {
            var postLabel = new FormattedText(string.Format("Путь {0}", Track.Number), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                              new Typeface("Verdana"), 10, mainBrush);
            dc.DrawText(postLabel, new Point());
            return postLabel.Height;
        }

        #region Реакция на движения мышью

        public override void OnMouseEnter(MouseEventArgs MouseEventArgs)
        {
            base.OnMouseEnter(MouseEventArgs);
            RequestChangeVisual();
        }

        public override void OnMouseLeave(MouseEventArgs MouseEventArgs)
        {
            base.OnMouseLeave(MouseEventArgs);
            RequestChangeVisual();
        }

        #endregion
    }
}
