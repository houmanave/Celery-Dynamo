using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryX.Nodes.UVPointer
{
    public class UVPointerNodeView : INodeViewCustomization<UVPointerNodeModel>
    {
        public void CustomizeView(UVPointerNodeModel model, NodeView nodeView)
        {
            var control = new UVPointerControl();
            nodeView.inputGrid.Children.Add(control);

            control.DataContext = model;
        }

        public void Dispose()
        {
        }
    }
}
