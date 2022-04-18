using CeleryX.CommonControls;
using CeleryX.Converters;
using Dynamo.Controls;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CeleryX.Nodes.GraphMap
{
    public class GraphMapNodeView : INodeViewCustomization<GraphMapNodeModel>
    {
        private DynamoViewModel dynamoViewModel = null;
        private DispatcherSynchronizationContext syncContext = null;
        private DynamoModel dynamoModel = null;
        private GraphMapNodeModel theModel;
        private GraphMapControl theControl;

        public void CustomizeView(GraphMapNodeModel model, NodeView nodeView)
        {
            dynamoViewModel = nodeView.ViewModel.DynamoViewModel;
            dynamoModel = dynamoViewModel.Model;
            theModel = model;
            syncContext = new DispatcherSynchronizationContext(nodeView.Dispatcher);

            theControl = new GraphMapControl(model);
            nodeView.inputGrid.Children.Add(theControl);

            theControl.DataContext = model;

            model.EngineController = nodeView.ViewModel.DynamoViewModel.EngineController;

            Rectangle theborder = new Rectangle();
            theborder.Margin = new System.Windows.Thickness(7);
            theborder.RadiusX = 5;
            theborder.RadiusY = 5;
            theborder.Stroke = Brushes.Gray;
            theborder.StrokeThickness = 2;
            Canvas.SetZIndex(theborder, 2);
            theControl.thisCanvas.Children.Add(theborder);

            //  linear curve
            Canvas.SetZIndex(model.PointLinear1, 74);
            Canvas.SetZIndex(model.PointLinear2, 74);
            theControl.thisCanvas.Children.Add(model.PointLinear1);
            theControl.thisCanvas.Children.Add(model.PointLinear2);

            model.CurveLinear = new CommonControls.Subcontrols.curvelinear(
                model.PointLinear1,
                model.PointLinear2,
                model.CanvasWidth,
                model.CanvasHeight);
            Canvas.SetZIndex(model.CurveLinear, 50);
            theControl.thisCanvas.Children.Add(model.CurveLinear.PathCurve);

            //  bezier curve
            Canvas.SetZIndex(model.PointBezierControl1, 60);
            Canvas.SetZIndex(model.PointBezierControl2, 60);
            Canvas.SetZIndex(model.PointBezierFix1, 75);
            Canvas.SetZIndex(model.PointBezierFix2, 75);
            theControl.thisCanvas.Children.Add(model.PointBezierControl1);
            theControl.thisCanvas.Children.Add(model.PointBezierControl2);
            theControl.thisCanvas.Children.Add(model.PointBezierFix1);
            theControl.thisCanvas.Children.Add(model.PointBezierFix2);

            model.CurveBezierControlLine1 = new CommonControls.Subcontrols.controlline(
                model.PointBezierControl1.Point,
                model.PointBezierFix1.Point);
            model.CurveBezierControlLine2 = new CommonControls.Subcontrols.controlline(
                model.PointBezierControl2.Point,
                model.PointBezierFix2.Point);
            model.CurveBezier = new CommonControls.Subcontrols.curvebezier(
                model.PointBezierFix1,
                model.PointBezierFix2,
                model.PointBezierControl1,
                model.PointBezierControl2,
                model.CanvasWidth,
                model.CanvasHeight);
            Canvas.SetZIndex(model.CurveBezierControlLine1, 25);
            Canvas.SetZIndex(model.CurveBezierControlLine2, 25);
            Canvas.SetZIndex(model.CurveBezier, 49);
            theControl.thisCanvas.Children.Add(model.CurveBezier.PathCurve);
            theControl.thisCanvas.Children.Add(model.CurveBezierControlLine1.PathCurve);
            theControl.thisCanvas.Children.Add(model.CurveBezierControlLine2.PathCurve);

            //  assign curves to cx points
            //  this makes the curves regeneratable when the points are dragged.
            model.PointLinear1.curvelin = model.CurveLinear;
            model.PointLinear2.curvelin = model.CurveLinear;

            model.PointBezierFix1.curvebez = model.CurveBezier;
            model.PointBezierFix2.curvebez = model.CurveBezier;
            model.PointBezierFix1.clinebez = model.CurveBezierControlLine1;
            model.PointBezierFix2.clinebez = model.CurveBezierControlLine2;
            model.PointBezierControl1.curvebez = model.CurveBezier;
            model.PointBezierControl2.curvebez = model.CurveBezier;
            model.PointBezierControl1.clinebez = model.CurveBezierControlLine1;
            model.PointBezierControl2.clinebez = model.CurveBezierControlLine2;

            //  binding to paths and points
            Binding bndlinear = new Binding("SelectedGraphType");
            bndlinear.Mode = BindingMode.TwoWay;
            bndlinear.Converter = new EnumGraphTypesConverter();    // IntegerToVisibilityConverter();
            bndlinear.ConverterParameter = GraphTypes.Linear;
            model.CurveLinear.PathCurve.SetBinding(Path.VisibilityProperty, bndlinear);
            model.PointLinear1.SetBinding(UserControl.VisibilityProperty, bndlinear);
            model.PointLinear2.SetBinding(UserControl.VisibilityProperty, bndlinear);

            model.CurveLinear.Regenerate(model.PointLinear1);
            model.CurveLinear.Regenerate(model.PointLinear2);

            Binding bndbezier = new Binding("SelectedGraphType");
            bndbezier.Mode = BindingMode.TwoWay;
            bndbezier.Converter = new EnumGraphTypesConverter();    // IntegerToVisibilityConverter();
            bndbezier.ConverterParameter = GraphTypes.Bezier;
            model.CurveBezier.PathCurve.SetBinding(Path.VisibilityProperty, bndbezier);
            model.CurveBezierControlLine1.PathCurve.SetBinding(Path.VisibilityProperty, bndbezier);
            model.CurveBezierControlLine2.PathCurve.SetBinding(Path.VisibilityProperty, bndbezier);
            model.PointBezierControl1.SetBinding(UserControl.VisibilityProperty, bndbezier);
            model.PointBezierControl2.SetBinding(UserControl.VisibilityProperty, bndbezier);
            model.PointBezierFix1.SetBinding(UserControl.VisibilityProperty, bndbezier);
            model.PointBezierFix2.SetBinding(UserControl.VisibilityProperty, bndbezier);

            //model.CurveBezier.Regenerate(model.PointBezierFix1);
            //model.CurveBezier.Regenerate(model.PointBezierFix2);
            //model.CurveBezier.Regenerate(model.PointBezierControl1);
            //model.CurveBezier.Regenerate(model.PointBezierControl2);
        }

        public void Dispose()
        {
        }
    }
}
