using System;
using System.Windows;
using System.Windows.Media;
using MapViewer.Emulation.Msul.ViewModels;

namespace MapViewer.Emulation.Msul.Views.Controls
{
    public class Carriage : FrameworkElement
    {
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (Double), typeof (Carriage),
            new FrameworkPropertyMetadata(default(Double), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof (Brush), typeof (Carriage), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof (Brush), typeof (Carriage), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof (CarriagePosition), typeof (Carriage), new PropertyMetadata(default(CarriagePosition)));

        static Carriage() { DefaultStyleKeyProperty.OverrideMetadata(typeof (Carriage), new FrameworkPropertyMetadata(typeof (Carriage))); }

        public CarriagePosition Position
        {
            get { return (CarriagePosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public Double StrokeThickness
        {
            get { return (Double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var pen = new Pen(Stroke, StrokeThickness);
            StreamGeometry geometry = GetGeometry();
            drawingContext.DrawGeometry(Fill, pen, geometry);
        }

        private StreamGeometry GetGeometry()
        {
            var streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                switch (Position)
                {
                    case CarriagePosition.Left:
                        DrawFace(geometryContext, 0, ActualWidth);
                        break;
                    case CarriagePosition.Right:
                        DrawFace(geometryContext, ActualWidth, 0);
                        break;
                    case CarriagePosition.Middle:
                        DrawMiddle(geometryContext);
                        break;
                }
            }
            return streamGeometry;
        }

        private void DrawMiddle(StreamGeometryContext geometryContext)
        {
            double t = StrokeThickness * 0.5;
            geometryContext.BeginFigure(new Point(ActualWidth - t, t), true, true);
            geometryContext.LineTo(new Point(t, t), true, true);
            geometryContext.LineTo(new Point(t, ActualHeight - t), true, true);
            geometryContext.LineTo(new Point(ActualWidth - t, ActualHeight - t), true, true);
            geometryContext.Close();
        }

        private void DrawFace(StreamGeometryContext geometryContext, double FaceX, double TailX)
        {
            double t = StrokeThickness * 0.5;
            double d = (TailX - FaceX) / Math.Abs(TailX - FaceX);
            geometryContext.BeginFigure(new Point(TailX - t, t), true, true);
            geometryContext.LineTo(new Point(FaceX + 0.8 * (TailX - FaceX) - t, t), true, true);
            geometryContext.BezierTo(new Point(FaceX, t),
                                     new Point(FaceX - t, ActualHeight - t),
                                     new Point(FaceX + d * 10 - t, ActualHeight - t),
                                     true, true);
            geometryContext.LineTo(new Point(TailX - t, ActualHeight - t), true, true);
        }
    }
}
