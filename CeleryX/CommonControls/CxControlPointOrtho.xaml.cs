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
    /// CxControlPointOrtho.xaml の相互作用ロジック
    /// </summary>
    public partial class CxControlPointOrtho : Thumb
    {
        private double OFFSETVALUE = 7;

        public bool IsVertical { get; set; }

        public double LimitWidth { get; set; }
        public double LimitHeight { get; set; }

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

        public curvebezier curvebez { get; set; }
        public controlline clinebez { get; set; }

        public CxControlPointOrtho(Point p, bool isVertical, double limitWidth, double limitHeight)
        {
            InitializeComponent();

            Point = p;

            Canvas.SetLeft(this, p.X - OFFSETVALUE);
            Canvas.SetTop(this, p.Y - OFFSETVALUE);

            LimitWidth = limitWidth;
            LimitHeight = limitHeight;

            IsVertical = isVertical;

            Canvas.SetZIndex(this, 10000);
        }

        public override string ToString()
        {
            return Point.X.ToString() + "," + Point.Y.ToString() + "," + IsVertical.ToString();
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double ehc = Canvas.GetLeft(this) + (IsVertical ? 0.0 : e.HorizontalChange) + OFFSETVALUE;
            double evc = Canvas.GetTop(this) + (IsVertical ? e.VerticalChange : 0.0) + OFFSETVALUE;

            if (ehc < 0)
            {
                ehc = 0;
            }
            else if (ehc > LimitWidth)
            {
                ehc = LimitWidth;
            }

            if (evc < 0)
            {
                evc = 0;
            }
            else if (evc > LimitHeight)
            {
                evc = LimitHeight;
            }

            Point = new Point(ehc, evc);

            Canvas.SetLeft(this, ehc - OFFSETVALUE);
            Canvas.SetTop(this, evc - OFFSETVALUE);

            //  update curves
            if (clinebez != null)
            {
                clinebez.Regenerate(this);
            }
            if (curvebez != null)
            {
                curvebez.Regenerate(this);
                double maxv = 0.0;
                double minv = 0.0;
                curvebez.GetMaximumMinimumOrdinates(LimitHeight, out minv, out maxv);
            }
        }
    }
}
