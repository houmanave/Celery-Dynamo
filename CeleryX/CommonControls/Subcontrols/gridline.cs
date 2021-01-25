using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CeleryX.CommonControls.Subcontrols
{
    public class Gridline : Curvebase
    {
        LineSegment lseg = null;

        public Gridline(double t, double canvasWidth, double canvasHeight, bool isVertical = true)
        {
            double ct = canvasWidth * t;
            Point p0 = new Point();
            Point p1 = new Point();
            if (isVertical)
            {
                p0 = new Point(ct, canvasHeight);
                p1 = new Point(ct, 0);
            }
            else
            {
                ct = canvasHeight * t;
                p0 = new Point(0, ct);
                p1 = new Point(canvasWidth, ct);
            }

            lseg = new LineSegment
            {
                Point = p1
            };
            PathFigure = new PathFigure();
            PathFigure.StartPoint = p0;
            PathFigure.Segments.Add(lseg);
            PathGeometry = new PathGeometry();
            PathGeometry.Figures.Add(PathFigure);

            PathCurve = new Path();
            PathCurve.Data = PathGeometry;

            if (isVertical)
            {
                PathCurve.Opacity = 0.35;
            }
            else
            {
                PathCurve.Opacity = 0.15;
            }
            PathCurve.Stroke = Brushes.LightGray;
            PathCurve.StrokeThickness = 1;

            Canvas.SetZIndex(this, 2);
        }
    }
}
