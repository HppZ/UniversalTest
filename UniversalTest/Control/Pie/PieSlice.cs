using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace AnimatedPieSlice
{
    public class PieSlice : Path
    {
        PathFigure pathFigure;
        ArcSegment arcSegment;

        static PieSlice()
        {
            CenterProperty = DependencyProperty.Register("TopCenter",
                typeof(Point), typeof(PieSlice),
                new PropertyMetadata(new Point(100, 0), OnPropertyChanged));

            RadiusProperty = DependencyProperty.Register("Radius",
                typeof(double), typeof(PieSlice),
                new PropertyMetadata(100.0, OnPropertyChanged));

            StartAngleProperty = DependencyProperty.Register("StartAngle",
                typeof(double), typeof(PieSlice),
                new PropertyMetadata(0.0, OnPropertyChanged));

            SweepAngleProperty = DependencyProperty.Register("SweepAngle",
                typeof(double), typeof(PieSlice),
                new PropertyMetadata(90.0, OnPropertyChanged));
        }

        public PieSlice()
        {
            pathFigure = new PathFigure { IsClosed = false ,IsFilled = false};
            arcSegment = new ArcSegment { SweepDirection = SweepDirection.Clockwise };
            pathFigure.Segments.Add(arcSegment);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            this.Data = pathGeometry;
            UpdateValues();
        }

        public static DependencyProperty CenterProperty { private set; get; }

        public static DependencyProperty RadiusProperty { private set; get; }

        public static DependencyProperty StartAngleProperty { private set; get; }

        public static DependencyProperty SweepAngleProperty { private set; get; }

        public Point TopCenter
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        public double Radius
        {
            set { SetValue(RadiusProperty, value); }
            get { return (double)GetValue(RadiusProperty); }
        }

        public double StartAngle
        {
            set { SetValue(StartAngleProperty, value); }
            get { return (double)GetValue(StartAngleProperty); }
        }

        public double SweepAngle
        {
            set { SetValue(SweepAngleProperty, value); }
            get { return (double)GetValue(SweepAngleProperty); }
        }

        static void OnPropertyChanged(DependencyObject obj, 
                                      DependencyPropertyChangedEventArgs args)
        {
            (obj as PieSlice).UpdateValues();
        }

        void UpdateValues()
        {
            pathFigure.StartPoint = this.TopCenter;
            double x = this.TopCenter.X + this.Radius * Math.Sin(Math.PI * (this.StartAngle +  this.SweepAngle) / 180);
            double y = this.TopCenter.Y + this.Radius - this.Radius * Math.Cos(Math.PI * (this.StartAngle +  this.SweepAngle) / 180);
            arcSegment.Point = new Point(x, y);
            arcSegment.IsLargeArc = this.SweepAngle >= 180;

            arcSegment.Size = new Size(this.Radius, this.Radius);
        }

        public void SetClosedAndFilled()
        {
            pathFigure.IsClosed = true;
            pathFigure.IsFilled = true;
        }

    }
}
