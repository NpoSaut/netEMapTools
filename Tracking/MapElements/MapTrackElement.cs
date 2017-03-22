using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace Tracking.MapElements
{
    public class MapTrackElement : MapPathElement
    {
        private readonly Vector _shadowVector = new Vector(0.8, 1.2);

        /// <summary>Создаёт новый многоточечный объект на карте</summary>
        /// <param name="Points">Точки, входящие в состав объекта</param>
        /// <param name="TrackPen">Карандаш для рисования трека</param>
        public MapTrackElement(IList<EarthPoint> Points, Pen TrackPen) : base(Points)
        {
            this.TrackPen = TrackPen;
            TrackShadowPen = new Pen(new SolidColorBrush(Color.FromArgb(80, 0, 0, 0)), TrackPen.Thickness);
        }

        public Pen TrackPen { get; set; }
        public Pen TrackShadowPen { get; set; }

        /// <summary>Z-индекс элемента на карте</summary>
        /// <remarks>Меньшее значения индекса соответствуют нижним слоям на карте</remarks>
        protected override int ZIndex
        {
            get { return -1; }
        }

        /// <summary>Отрисовывает объект в указанном контексте рисования</summary>
        /// <param name="dc">Контекст рисования</param>
        /// <param name="Zoom">Индекс масштаба рисования</param>
        protected override void Draw(DrawingContext dc, int Zoom)
        {
            if (Points.Count == 1)
            {
                dc.DrawEllipse(TrackPen.Brush, null, Projector.Project(Points[0], Zoom), TrackPen.Thickness * 1.5, TrackPen.Thickness * 1.5);
                return;
            }

            var points = GetScreenPoints(Zoom).ToList();

            for (var i = 0; i < points.Count - 1; i++)
                dc.DrawLine(TrackShadowPen,
                            points[i] + _shadowVector,
                            points[i + 1] + _shadowVector);

            for (var i = 0; i < points.Count - 1; i++)
                dc.DrawLine(TrackPen, points[i], points[i + 1]);
        }
    }
}
