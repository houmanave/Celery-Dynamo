using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryX.Nodes.InputBoundedNumberSlider
{
    public class InputBoundedNumberSliderNodeView : INodeViewCustomization<InputBoundedNumberSliderNodeModel>
    {
        public void CustomizeView(InputBoundedNumberSliderNodeModel model, NodeView nodeView)
        {
            var control = new InputBoundedNumberSliderControl();
            nodeView.inputGrid.Children.Add(control);

            control.DataContext = model;
        }

        public void Dispose()
        { 
        }
    }
}
