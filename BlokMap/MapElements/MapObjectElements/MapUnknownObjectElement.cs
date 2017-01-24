using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Geographics;
using GMapElements;

namespace BlokMap.MapElements.MapObjectElements
{
    public class MapUnknownObjectElement : MapObjectElement
    {
        private const Double Size = 8;

        private static readonly Dictionary<GObjectType, string> _typeNames =
            new Dictionary<GObjectType, string>
            {
                { GObjectType.Bridge, "Мост" },
                { GObjectType.Crossing, "Переезд" },
                { GObjectType.DangerousPlace, "Огр." },
                { GObjectType.DeadEnd, "Тупик" },
                { GObjectType.GpuSaut, "Генератор" },
                { GObjectType.Platform, "Платформа" },
                { GObjectType.Station, "Станция" },
                { GObjectType.Switch, "Стрелка" },
                { GObjectType.Tks, "ТКС" },
                { GObjectType.TrafficLight, "Светофор" },
                { GObjectType.Tunnel, "Тоннель" },
                { GObjectType.Unknown, "Неизвестный" }
            };

        public MapUnknownObjectElement(EarthPoint Position, GObject Target) : base(Position, Target) { }

        protected override void DrawPointElement(DrawingContext dc, int Zoom)
        {
            double bw = 1;
            var accentPen = new Pen(Brushes.Red, 2);
            var backgroundPen = new Pen(Brushes.White, 2 + bw * 2);
            dc.DrawLine(backgroundPen, new Point(-0.5 * Size - bw, -0.5 * Size - bw), new Point(0.5 * Size + bw, 0.5 * Size + bw));
            dc.DrawLine(backgroundPen, new Point(0.5 * Size + bw, -0.5 * Size - bw), new Point(-0.5 * Size - bw, 0.5 * Size + bw));
            dc.DrawLine(accentPen, new Point(-0.5 * Size, -0.5 * Size), new Point(0.5 * Size, 0.5 * Size));
            dc.DrawLine(accentPen, new Point(0.5 * Size, -0.5 * Size), new Point(-0.5 * Size, 0.5 * Size));

            if (Zoom > 12)
                PrintDetails(dc, String.Format("{0} \"{1}\"", _typeNames[Target.Type], Target.Name));
        }
    }
}
