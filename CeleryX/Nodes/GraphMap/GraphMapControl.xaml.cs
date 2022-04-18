using CeleryX.CommonControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CeleryX.Nodes.GraphMap
{
    /// <summary>
    /// GraphMapControl.xaml の相互作用ロジック
    /// </summary>
    public partial class GraphMapControl : UserControl, INotifyPropertyChanged
    {
        const double POINTHALF = 7.0;
        const double HEIGHTADJ = 64.0;
        const double WIDTHADJ = 16.0;

        public GraphMapControl(GraphMapNodeModel model)
        {
            InitializeComponent();

            model.PropertyChanged += Model_PropertyChanged;

            DataContext = this;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //  If there are some internal values you want to change in the UI, do it here.
            if (e.PropertyName == "DataUpdated")
            {
                var model = sender as GraphMapNodeModel;
                this.Dispatcher.Invoke(() =>
                {
                    //model.LeftLimitValue;
                    //model.RightLimitValue;
                    //model.SliderValue;
                }
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CxMovablePointFree_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var ehc = e.HorizontalChange;
            var evc = e.VerticalChange;

            if (this.Parent.GetType() == typeof(Grid))
            {
                var inputgrid = this.Parent as Grid;

                Canvas theCanvas = null;
                foreach (var child in inputgrid.Children)
                {
                    if (child.GetType() == typeof(GraphMapControl))
                    {
                        GraphMapControl gmcont = child as GraphMapControl;
                        theCanvas = gmcont.thisCanvas;
                    }
                }

                if (theCanvas != null)
                {
                    var xadj = ActualWidth + e.HorizontalChange;
                    var yadj = ActualHeight + e.VerticalChange;

                    bool isxadj = false;
                    bool isyadj = false;
                    double oldCvWidth = theCanvas.ActualWidth;
                    double oldCvHeight = theCanvas.ActualHeight;

                    isxadj = (xadj) >= theCanvas.MinWidth + WIDTHADJ;
                    if (isxadj)
                    {
                        Width = xadj;
                    }
                    else
                    {
                        oldCvWidth = theCanvas.MinWidth;
                    }

                    isyadj = (yadj) >= theCanvas.MinHeight + HEIGHTADJ;
                    if (isyadj)
                    {
                        Height = yadj;
                    }
                    else
                    {
                        oldCvHeight = theCanvas.MinHeight;
                    }

                    foreach (var item in theCanvas.Children)
                    {
                        CxControlPointFree cxptfree = item as CxControlPointFree;
                        CxControlPointOrtho cxptortho = item as CxControlPointOrtho;
                        System.Windows.Shapes.Path curve = item as System.Windows.Shapes.Path;
                        if (cxptfree != null)
                        {
                            double newx = Canvas.GetLeft(cxptfree) + POINTHALF;
                            double newy = Canvas.GetTop(cxptfree) + POINTHALF;
                            if (isxadj)
                            {
                                double x = newx;
                                double dx = x * ehc / oldCvWidth;
                                newx = x + dx;
                                double xfin = newx - POINTHALF;
                                Canvas.SetLeft(cxptfree, xfin);

                                cxptfree.LimitWidth = theCanvas.ActualWidth + e.HorizontalChange;
                                if (cxptfree.curvelin != null)
                                {
                                    cxptfree.curvelin.MaxWidth = theCanvas.ActualWidth + e.HorizontalChange;
                                }
                            }

                            if (isyadj)
                            {
                                double y = newy;
                                double dy = y * evc / oldCvHeight;
                                newy = y + dy;
                                double yfin = newy - POINTHALF;
                                Canvas.SetTop(cxptfree, yfin);

                                cxptfree.LimitHeight = theCanvas.ActualHeight + e.VerticalChange;
                                if (cxptfree.curvelin != null)
                                {
                                    cxptfree.curvelin.MaxHeight = theCanvas.ActualHeight + e.VerticalChange;
                                }
                            }

                            //if (cxptfree.clinebez != null)
                            //    cxptfree.clinebez.Regenerate(cxptfree);
                            //if (cxptfree.curvebez != null)
                            //    cxptfree.curvebez.Regenerate(cxptfree);
                            //if (cxptfree.curvelin != null)
                            //    cxptfree.curvelin.Regenerate(cxptfree);
                        }
                        else if (cxptortho != null)
                        {
                            double newx = Canvas.GetLeft(cxptortho) + POINTHALF;
                            double newy = Canvas.GetTop(cxptortho) + POINTHALF;
                            if (isxadj)
                            {
                                double x = newx;
                                double dx = x * ehc / oldCvWidth;
                                newx = x + dx;
                                double xfin = newx - POINTHALF;
                                Canvas.SetLeft(cxptortho, xfin);

                                cxptortho.LimitWidth = theCanvas.ActualWidth + e.HorizontalChange;
                            }

                            if (isyadj)
                            {
                                double y = newy;
                                double dy = y * evc / oldCvHeight;
                                newy = y + dy;
                                double yfin = newy - POINTHALF;
                                Canvas.SetTop(cxptortho, yfin);

                                cxptortho.LimitHeight = theCanvas.ActualHeight + e.VerticalChange;
                            }

                            //if (cxptortho.curvebez != null)
                            //    cxptortho.curvebez.Regenerate(cxptortho);
                        }
                        else if (curve != null)
                        {
                            PathGeometry geodata = curve.Data as PathGeometry;
                            if (geodata == null)
                            {
                                continue;
                            }
                            foreach (PathFigure fig in geodata.Figures)
                            {
                                Point stpt = fig.StartPoint;
                                double sx = stpt.X;
                                double sdx = (isxadj) ? sx * ehc / oldCvWidth : 0.0;
                                double sy = stpt.Y;
                                double sdy = (isyadj) ? sy * evc / oldCvHeight : 0.0;
                                fig.StartPoint = new Point(sx + sdx, sy + sdy);
                                foreach (PathSegment seg in fig.Segments)
                                {
                                    LineSegment lseg = seg as LineSegment;
                                    BezierSegment bseg = seg as BezierSegment;
                                    if (lseg != null)
                                    {
                                        Point lpt = lseg.Point;
                                        double lx = lpt.X;
                                        double ldx = (isxadj) ? lx * ehc / oldCvWidth : 0.0;
                                        double ly = lpt.Y;
                                        double ldy = (isyadj) ? ly * evc / oldCvHeight : 0.0;
                                        lseg.Point = new Point(lx + ldx, ly + ldy);
                                    }
                                    else if (bseg != null)
                                    {
                                        Point bpt1 = bseg.Point1;
                                        double lx = bpt1.X;
                                        double ldx = (isxadj) ? lx * ehc / oldCvWidth : 0.0;
                                        double ly = bpt1.Y;
                                        double ldy = (isyadj) ? ly * evc / oldCvHeight : 0.0;
                                        bseg.Point1 = new Point(lx + ldx, ly + ldy);

                                        Point bpt2 = bseg.Point2;
                                        lx = bpt2.X;
                                        ldx = (isxadj) ? lx * ehc / oldCvWidth : 0.0;
                                        ly = bpt2.Y;
                                        ldy = (isyadj) ? ly * evc / oldCvHeight : 0.0;
                                        bseg.Point2 = new Point(lx + ldx, ly + ldy);

                                        Point bpt3 = bseg.Point3;
                                        lx = bpt3.X;
                                        ldx = (isxadj) ? lx * ehc / oldCvWidth : 0.0;
                                        ly = bpt3.Y;
                                        ldy = (isyadj) ? ly * evc / oldCvHeight : 0.0;
                                        bseg.Point3 = new Point(lx + ldx, ly + ldy);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
