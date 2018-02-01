using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Geographics;
using GMapElements;
using GMapElements.Entities;
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
                { AlsnFrequency.Unknown, "Неизв." }
            };

        public MapObjectElement(EarthPoint Position, GObject Target) : base(Position) { this.Target = Target; }

        public GObject Target { get; set; }

        protected override int ZIndex
        {
            get { return base.ZIndex + (IsMouseOver ? 100 : 0); }
        }

        protected string OrdinateString
        {
            get { return string.Format("{0:F3}", Target.Ordinate / 1000.0); }
        }

        protected string AlstFrequencyString
        {
            get { return _frequencyNames[Target.AlsnFreq]; }
        }

        protected void PrintDetails(DrawingContext dc, string Name = null)
        {
            IList<FormattedText> stack = new List<FormattedText>();

            if (!string.IsNullOrWhiteSpace(Name))
                stack.Add(new FormattedText(Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface(new FontFamily("Verdana"), FontStyles.Normal, IsMouseOver ? FontWeights.SemiBold : FontWeights.Medium,
                                                         FontStretches.Normal), 10,
                                            Brushes.Black));

            if (IsMouseOver)
            {
                stack.Add(new FormattedText(OrdinateString, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                                            new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 10,
                                            Brushes.Black));
                if (Target.Length > 0)
                    stack.Add(new FormattedText(string.Format("Длина: {0}м", Target.Length), CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                                                new Typeface(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 9,
                                                Brushes.Black));
                stack.Add(new FormattedText(string.Format("АЛСН: {0}", AlstFrequencyString), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface("Verdana"), 10, Brushes.Black));
                stack.Add(new FormattedText(string.Format("{0}: {1}км/ч", Target.Type == GObjectType.TrafficLight ? "На Ж" : "Огр.", Target.SpeedRestriction),
                                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface("Verdana"), 10, Brushes.Black));
            }

            if (stack.Any())
            {
                dc.PushTransform(new TranslateTransform(Math.Round(-0.5 * stack.Max(l => l.Width)), 5));
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
