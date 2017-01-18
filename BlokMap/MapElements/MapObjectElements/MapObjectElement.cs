using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;
using MapVisualization.Elements;

namespace BlokMap.MapElements.MapObjectElements
{
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

        protected static readonly SolidColorBrush TextBackgroundBrush = new SolidColorBrush(Colors.White) { Opacity = 0.7 };
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
                             new Rect(-2, -1, labels.Max(l => l.Width) + 2, labels.Sum(l => l.Height + 1) + 2));
            double yOffset = 0;
            foreach (FormattedText label in labels)
            {
                dc.DrawText(label, new Point(0, yOffset));
                yOffset += label.Height + 1;
            }
        }
    }
}
