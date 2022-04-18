using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CeleryX.CommonControls.Subcontrols
{
    public class curvelinear : Curvebase
    {
        LineSegment lseg = null;

        public CxControlPointFree tcf1 { get; set; }
        public CxControlPointFree tcf2 { get; set; }

        public double MaxWidth { get; set; }

        public double MaxHeight { get; set; }



        public curvelinear(CxControlPointFree f1, CxControlPointFree f2, double maxw, double maxh)
        {
            tcf1 = f1;
            tcf2 = f2;

            MaxWidth = maxw;
            MaxHeight = maxh;

            lseg = new LineSegment(f2.Point, true);

            PathFigure = new PathFigure();
            PathFigure.StartPoint = f1.Point;
            PathFigure.Segments.Add(lseg);

            PathGeometry = new PathGeometry();
            PathGeometry.Figures.Add(PathFigure);

            PathCurve = new System.Windows.Shapes.Path();
            PathCurve.Data = PathGeometry;

            PathCurve.Stroke = Brushes.Cornsilk;
            PathCurve.StrokeThickness = 3;
            PathCurve.Opacity = 0.9;
        }

        private double LineEquation(double x)
        {
            double m = (tcf2.Point.Y - tcf1.Point.Y) / (tcf2.Point.X - tcf1.Point.X);
            if (double.IsNaN(m))
            {
                return double.NaN;
            }

            //  y - y1 = m(x - x1)
            return m * (x - tcf1.Point.X) + (tcf1.Point.Y);
        }

        private double SolveForXGivenY(double y)
        {
            double m = (tcf2.Point.Y - tcf1.Point.Y) / (tcf2.Point.X - tcf1.Point.X);
            if (double.IsNaN(m))
            {
                return double.NaN;
            }

            //  x = ((y - y1) / m + x1
            return ((y - tcf1.Point.Y) / m) + tcf1.Point.X;
        }

        public override List<double> GetValuesFromAssignedParameters(double lowLimit, double highLimit, int count)
        {
            if (count < 1)
                return null;

            List<double> livalues = new List<double>();

            //  test verticality
            if (tcf1.Point.X == tcf2.Point.X)
            {
                return null;
            }
            if (double.IsNaN(LineEquation(0.0)))
            {
                return null;
            }

            double incount = (double)(count - 1);
            for (double d = 0.0; d < MaxWidth; d += (MaxWidth / incount))
            {
                double md = MaxHeight - LineEquation(d);
                double rd = (highLimit - lowLimit) * md / MaxHeight;
                rd += lowLimit;
                livalues.Add(rd);
            }

            if (livalues.Count < count)
            {
                double mdx = MaxHeight - LineEquation(MaxWidth);
                double rdx = (highLimit - lowLimit) * mdx / MaxHeight;
                rdx += lowLimit;
                livalues.Add(rdx);
            }

            return livalues;
        }

        public void Regenerate(CxControlPointFree f)
        {
            if (tcf1 == f)
            {
                tcf1 = f;
            }
            else if (tcf2 == f)
            {
                tcf2 = f;
            }

            double y01 = LineEquation(0);
            double y02 = LineEquation(MaxWidth);
            if (double.IsNaN(y01) || double.IsNaN(y02))
            {

            }
            else
            {
                Point p;
                if (y01 < 0)
                {
                    p = new Point(SolveForXGivenY(0), 0);
                }
                else if (y01 > MaxHeight)
                {
                    p = new Point(SolveForXGivenY(MaxHeight), MaxHeight);
                }
                else
                {
                    p = new Point(0, y01);
                }
                PathFigure.StartPoint = p;

                Point q;
                if (y02 < 0)
                {
                    q = new Point(SolveForXGivenY(0), 0);
                }
                else if (y02 > MaxHeight)
                {
                    q = new Point(SolveForXGivenY(MaxHeight), MaxHeight);
                }
                else
                {
                    q = new Point(MaxWidth, y02);
                }
                lseg.Point = q;
            }
        }
    }
}
