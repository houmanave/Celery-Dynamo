using Dynamo.Controls;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CeleryX.Nodes.Valve
{
    public class ValveNodeView : INodeViewCustomization<ValveNodeModel>
    {
        private ValveNodeModel nodeModel;
        private ValveControl control;

        public void CustomizeView(ValveNodeModel model, NodeView nodeView)
        {
            var valve = new ValveControl();
            nodeView.inputGrid.Children.Add(valve);
            valve.DataContext = model;

            control = valve;
            nodeModel = model;

            //model.IsOpenClose = 0;
        }

        public void Dispose()
        {
        }
    }
}
