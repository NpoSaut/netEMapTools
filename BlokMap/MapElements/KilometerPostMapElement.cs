using System;
using System.Collections.Generic;
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
        private static readonly Dictionary<PositionInSection, string> _positionNames =
            new Dictionary<PositionInSection, string>
            {
                { PositionInSection.Start, "Начало" },
                { PositionInSection.Middle, "Середина" },
                { PositionInSection.End, "Конец" }
            };

        private readonly Brush _mainBrush = Brushes.DarkSlateGray;

        public KilometerPostMapElement(GPost Post) : base(Post.Point)
        {
            this.Post = Post;
            SectionBrush = Brushes.Aquamarine;
        }

        public GPost Post { get; private set; }

        public Brush SectionBrush { get; set; }

        protected override int ZIndex
        {
            get { return base.ZIndex + (IsMouseOver ? 100 : 0); }
        }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            if (Zoom > 12 || IsMouseOver)
            {
                var postLabelText = string.Format("{0}км", (double)Post.Ordinate / 1000);
                var postLabel = new FormattedText(postLabelText, CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight, new Typeface("Verdana"), 10, _mainBrush);

                const int flagHeight = 22;
                dc.PushTransform(new TranslateTransform(0, -flagHeight));

                dc.DrawRectangle(Brushes.White, new Pen(_mainBrush, 1), new Rect(-0.5, -0.5, Math.Round(postLabel.Width) + 5, Math.Round(postLabel.Height) + 2));
                dc.DrawText(postLabel, new Point(2, 0));
                dc.DrawLine(new Pen(_mainBrush, 2), new Point(0, 0), new Point(0, flagHeight));

                dc.Pop();
            }

            if (Zoom > 8)
            {
                dc.DrawEllipse(SectionBrush, new Pen(_mainBrush, 1.5), new Point(0, 0), 5, 5);
            }
            else
            {
                if (Post.Ordinate % (int)(1000 * Math.Pow(2, 9 - Zoom)) == 0)
                    dc.DrawRectangle(SectionBrush, null, new Rect(-2, -2, 4, 4));
            }

            if (IsMouseOver)
            {
                dc.PushTransform(new TranslateTransform(3, 10));
                PrintStack(dc,
                           new FormattedText(string.Format("Секция #{0}", Post.SectionId),
                                             CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Sienna),
                           new FormattedText(Post.Direction == OrdinateDirection.Increasing ? "Возрастает по неч." : "Убывает по неч.",
                                             CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black),
                           new FormattedText(string.Format("Пути: {0}", string.Join(", ", Post.Tracks.Select(TrackName))),
                                             CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black),
                           new FormattedText(string.Format("Положение: {0}", _positionNames[Post.Position]),
                                             CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black));
                dc.Pop();
            }
        }

        private string TrackName(GTrack Track)
        {
            if (Track.Number <= 15)
                return string.Format("{0}П", Track.Number);
            return string.Format("{0}Н", Track.Number - 15);
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
