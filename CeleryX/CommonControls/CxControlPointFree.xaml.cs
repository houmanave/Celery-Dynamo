using CeleryX.CommonControls.Subcontrols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CeleryX.CommonControls
{
    /// <summary>
    /// CxControlPointFree.xaml の相互作用ロジック
    /// </summary>
    public partial class CxControlPointFree : Thumb
    {
        private double OFFSETVALUE = 7;

        public Crosshair chairhor { get; set; }
        public Crosshair chairver { get; set; }
        public Uvcoordtext uvtext { get; set; }

        public double maxwidth { get; set; }
        public double maxheight { get; set; }

        private Point point;
        public Point Point
        {
            get
            {
                return point;
            }

            set
            {
                point = value;
                Canvas.SetLeft(this, point.X - OFFSETVALUE);
                Canvas.SetTop(this, point.Y - OFFSETVALUE);
            }
        }

        public CxControlPointFree(Point p)
        {
            InitializeComponent();

            Point = p;

            Canvas.SetLeft(this, p.X - OFFSETVALUE);
            Canvas.SetTop(this, p.Y - OFFSETVALUE);
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double ehc = Canvas.GetLeft(this) + e.HorizontalChange + OFFSETVALUE;
            double evc = Canvas.GetTop(this) + e.VerticalChange + OFFSETVALUE;

            if (ehc < 0)
            {
                ehc = 0;
            }
            else if (ehc > maxwidth)
            {
                ehc = maxwidth;
            }

            if (evc < 0)
            {
                evc = 0;
            }
            else if (evc > maxheight)
            {
                evc = maxheight;
            }

            Point = new Point(ehc, evc);

            Canvas.SetLeft(this, ehc - OFFSETVALUE);
            Canvas.SetTop(this, evc - OFFSETVALUE);

            //if (scurve != null)
            //{
            //    scurve.Regenerate(this);
            //}

            //if (lcurve != null)
            //{
            //    lcurve.Regenerate(this);
            //}

            //if (parabcurve != null)
            //{
            //    parabcurve.Regenerate(this);
            //}

            //if (perlcurve != null)
            //{
            //    perlcurve.Regenerate(this);
            //}

            if (chairhor != null)
            {
                chairhor.Regenerate(this);
            }
            if (chairver != null)
            {
                chairver.Regenerate(this);
            }
            if (uvtext != null)
            {
                uvtext.Regenerate(Point);
            }
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }

        public override string ToString()
        {
            return Point.X.ToString() + "," + Point.Y.ToString();
        }

        public void Update(double ehc, double evc)
        {
            if (ehc < 0)
            {
                ehc = 0;
            }
            else if (ehc > maxwidth)
            {
                ehc = maxwidth;
            }

            if (evc < 0)
            {
                evc = 0;
            }
            else if (evc > maxheight)
            {
                evc = maxheight;
            }

            Point = new Point(ehc, evc);

            Canvas.SetLeft(this, ehc - OFFSETVALUE);
            Canvas.SetTop(this, evc - OFFSETVALUE);

            //if (lcurve != null)
            //{
            //    lcurve.ReconfigureWidthAndHeight(maxwidth, maxheight);
            //    lcurve.Regenerate(this);
            //}

            //if (scurve != null)
            //{
            //    scurve.Regenerate(this);
            //    double maxv = 0.0;
            //    double minv = 0.0;
            //    scurve.GetMaximumMinimumOrdinates(maxheight, out minv, out maxv);
            //}
        }
    }
}
