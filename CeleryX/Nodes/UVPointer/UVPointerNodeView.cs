using CeleryX.CommonControls;
using CeleryX.CommonControls.Subcontrols;
using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CeleryX.Nodes.UVPointer
{
    public class UVPointerNodeView : INodeViewCustomization<UVPointerNodeModel>
    {
        UVPointerNodeModel UvNodeModel;

        public void CustomizeView(UVPointerNodeModel model, NodeView nodeView)
        {
            var control = new UVPointerControl();
            nodeView.inputGrid.Children.Add(control);

            control.DataContext = model;

            UvNodeModel = model;

            double thewidth = control.thisCanvas.Width;
            double theheight = control.thisCanvas.Height;

            if (thewidth == 0.0 || double.IsNaN(thewidth))
                thewidth = model.CanvasWidth;
            else
                model.CanvasWidth = thewidth;

            if (theheight == 0.0 || double.IsNaN(theheight))
                theheight = model.CanvasHeight;
            else
                model.CanvasHeight = theheight;

            //
            model.pointMover = new CxControlPointFree(new System.Windows.Point(model.U, model.V));
            model.pointMover.maxwidth = thewidth;
            model.pointMover.maxheight = theheight;
            Canvas.SetZIndex(model.pointMover, 75);

            model.CrossHairHorizontal = new Crosshair(control.thisCanvas, model.pointMover.Point, thewidth, theheight);
            model.CrossHairVertical = new Crosshair(control.thisCanvas, model.pointMover.Point, thewidth, theheight, true);
            model.UVCoordinateText = new Uvcoordtext(model.pointMover.Point, thewidth, theheight);

            control.thisCanvas.Children.Add(model.pointMover);
            control.thisCanvas.Children.Add(model.CrossHairHorizontal.PathCurve);
            control.thisCanvas.Children.Add(model.CrossHairVertical.PathCurve);
            control.thisCanvas.Children.Add(model.UVCoordinateText);

            foreach (Gridline gl in model.GridLines)
            {
                control.thisCanvas.Children.Add(gl.PathCurve);
                Canvas.SetZIndex(gl.PathCurve, 10);
            }

            model.pointMover.chairhor = model.CrossHairHorizontal;
            model.pointMover.chairver = model.CrossHairVertical;
            model.pointMover.uvtext = model.UVCoordinateText;

            model.pointMover.PreviewMouseLeftButtonUp += CanvasPreviewMouseLeftUp;

            UpdateUVPointer(model.pointMover);
        }

        private void CanvasPreviewMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CxControlPointFree cecf = sender as CxControlPointFree;

            UpdateUVPointer(cecf);
        }

        private void UpdateUVPointer(CxControlPointFree cecf)
        {
            if (cecf != null)
            {
                if (cecf.chairhor != null)
                    cecf.chairhor.Regenerate(cecf);
                if (cecf.chairver != null)
                    cecf.chairver.Regenerate(cecf);
                if (cecf.uvtext != null)
                {
                    cecf.uvtext.Regenerate(cecf.Point);
                }

                UvNodeModel.U = cecf.Point.X;
                UvNodeModel.V = cecf.Point.Y;
            }

            UvNodeModel.OnNodeModified();
        }

        public void Dispose()
        {
            UvNodeModel.pointMover.PreviewMouseLeftButtonUp -= CanvasPreviewMouseLeftUp;
        }
    }
}
