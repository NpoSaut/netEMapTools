using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Geographics;
using GMapElements;
using MapVisualization.Elements;

namespace BlokMap.MapElements.MapObjectElements
{
    [ZoomRestriction(12)]
    public abstract class MapObjectElement : MapPointElement
    {
        private static readonly Dictionary<AlsnFrequency, string> _frequencyNames =
            new Dictionary<AlsnFrequency, string>
            {
                { AlsnFrequency.Alsn25, "25Гц" },
                { AlsnFrequency.Alsn50, "50Гц" },
                { AlsnFrequency.Alsn75, "75Гц" },
                { AlsnFrequency.NoAlsn, "Без АЛСН" },
                { AlsnFrequency.Unknown, "Неизв." },
            };

        protected static readonly SolidColorBrush TextBackgroundBrush = new SolidColorBrush(Colors.White) { Opacity = 0.8 };

        public MapObjectElement(EarthPoint Position, GObject Target) : base(Position) { this.Target = Target; }

        public GObject Target { get; set; }

        protected string OrdinateString
        {
            get { return string.Format("{0:F3}", Target.Ordinate / 1000.0); }
        }

        protected string AlstFrequencyString
        {
            get { return _frequencyNames[Target.AlsnFreq]; }
        }

        protected static void PrintStack(DrawingContext dc, params FormattedText[] labels) { PrintStack(dc, (IList<FormattedText>)labels); }

        protected static void PrintStack(DrawingContext dc, IList<FormattedText> labels)
        {
            dc.DrawRectangle(TextBackgroundBrush, null,
                             new Rect(-2, -1, labels.Max(l => l.Width) + 4, labels.Sum(l => l.Height + 1) + 2));
            double yOffset = 0;
            foreach (FormattedText label in labels)
            {
                dc.DrawText(label, new Point(0, yOffset));
                yOffset += label.Height + 1;
            }
        }

        protected void PrintDetails(DrawingContext dc, string Name = null)
        {
            IList<FormattedText> stack = new List<FormattedText>();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                stack.Add(new FormattedText(Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Medium, FontStretches.Normal), 10,
                                            Brushes.Black));
            }

            if (IsMouseOver)
            {
                stack.Add(new FormattedText(OrdinateString, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                                            new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 10,
                                            Brushes.Black));
                if (Target.Length > 0)
                {
                    stack.Add(new FormattedText(String.Format("Длина: {0}м", Target.Length), CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                                                new Typeface(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 9,
                                                Brushes.Black));
                }
                stack.Add(new FormattedText(String.Format("АЛСН: {0}", AlstFrequencyString), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface("Verdana"), 10, Brushes.Black));
                stack.Add(new FormattedText(String.Format("{0}: {1}км/ч", Target.Type == GObjectType.TrafficLight ? "На Ж" : "Огр.", Target.SpeedRestriction),
                                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface("Verdana"), 10, Brushes.Black));
            }

            if (stack.Any())
            {
                dc.PushTransform(new TranslateTransform(-0.5 * stack.Max(l => l.Width), 5));
                PrintStack(dc, stack);
                dc.Pop();
            }
        }

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
    }
}
