using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CeleryX.CommonControls.Subcontrols
{
    public class controlline : Curvebase
    {
        LineSegment lseg = null;

        public controlline(Point sp, Point ep)
        {
            lseg = new LineSegment(ep, true);

            PathFigure = new PathFigure();
            PathFigure.StartPoint = sp;
            PathFigure.Segments.Add(lseg);

            PathGeometry = new PathGeometry();
            PathGeometry.Figures.Add(PathFigure);

            PathCurve = new System.Windows.Shapes.Path();
            PathCurve.Data = PathGeometry;

            PathCurve.Stroke = Brushes.LightGray;
            PathCurve.StrokeThickness = 2;
            PathCurve.Opacity = 0.7;

            PathCurve.StrokeDashArray.Add(3);
            PathCurve.StrokeDashArray.Add(1);
        }

        public void Regenerate(CxControlPointFree s)
        {
            PathFigure.StartPoint = s.Point;
        }

        public void Regenerate(CxControlPointOrtho c)
        {
            lseg.Point = c.Point;
        }
    }
}
