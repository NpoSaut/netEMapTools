using System.Collections.Generic;
using System.Windows.Media;
using Geographics;
using MapVisualization.Elements;

namespace Tracking.MapElements
{
    public class MapTrackElement : MapPathElement
    {
        /// <summary>Создаёт новый многоточечный объект на карте</summary>
        /// <param name="Points">Точки, входящие в состав объекта</param>
        /// <param name="TrackPen">Карандаш для рисования трека</param>
        public MapTrackElement(IList<EarthPoint> Points, Pen TrackPen) : base(Points) { this.TrackPen = TrackPen; }

        public Pen TrackPen { get; set; }

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
            for (int i = 0; i < Points.Count - 1; i++)
            {
                dc.DrawLine(TrackPen,
                            Projector.Project(Points[i], Zoom),
                            Projector.Project(Points[i + 1], Zoom));
            }
        }
    }
}
