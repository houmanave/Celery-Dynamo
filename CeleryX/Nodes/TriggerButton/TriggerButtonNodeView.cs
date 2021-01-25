using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryX.Nodes.TriggerButton
{
    public class TriggerButtonNodeView : INodeViewCustomization<TriggerButtonNodeModel>
    {
        private TriggerButtonControl control;
        private TriggerButtonNodeModel nodeModel;

        public void CustomizeView(TriggerButtonNodeModel model, NodeView nodeView)
        {
            var button = new TriggerButtonControl();
            nodeView.inputGrid.Children.Add(button);
            button.DataContext = model;

            control = button;
            nodeModel = model;

            button.theButton.PreviewMouseLeftButtonUp += ButtonPreviewMouseLeftUp;
            button.theButton.PreviewMouseLeftButtonDown += ButtonPreviewMouseLeftDown;
        }

        private void ButtonPreviewMouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            nodeModel.TriggerValue = true;
        }

        private void ButtonPreviewMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            nodeModel.TriggerValue = false;
        }

        public void Dispose()
        {
            control.theButton.PreviewMouseLeftButtonUp -= ButtonPreviewMouseLeftUp;
            control.theButton.PreviewMouseLeftButtonDown -= ButtonPreviewMouseLeftDown;
        }
    }
}
