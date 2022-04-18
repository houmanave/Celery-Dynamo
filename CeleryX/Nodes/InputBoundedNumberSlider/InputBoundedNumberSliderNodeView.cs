using Dynamo.Controls;
using Dynamo.Models;
using Dynamo.Scheduler;
using Dynamo.ViewModels;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CeleryX.Nodes.InputBoundedNumberSlider
{
    public class InputBoundedNumberSliderNodeView : INodeViewCustomization<InputBoundedNumberSliderNodeModel>
    {
        private DynamoViewModel dynamoViewModel = null;
        private DispatcherSynchronizationContext syncContext = null;
        private DynamoModel dynamoModel = null;
        private InputBoundedNumberSliderNodeModel theModel;
        private InputBoundedNumberSliderControl theControl;

        public void CustomizeView(InputBoundedNumberSliderNodeModel model, NodeView nodeView)
        {
            dynamoViewModel = nodeView.ViewModel.DynamoViewModel;
            dynamoModel = dynamoViewModel.Model;
            theModel = model;
            syncContext = new DispatcherSynchronizationContext(nodeView.Dispatcher);

            theControl = new InputBoundedNumberSliderControl(model);
            nodeView.inputGrid.Children.Add(theControl);

            theControl.DataContext = model;

            model.EngineController = nodeView.ViewModel.DynamoViewModel.EngineController;

            UpdateVariableNumberSlider();

            theControl.slider.PreviewMouseUp += ControlSliderPreviewMouseUp;
            theControl.slider.PreviewMouseDown += ControlSliderPreviewMouseDown;
            theControl.slider.ValueChanged += ControlSliderValueChanged;
        }

        private void ControlSliderValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            //  update values while scrolling.
            UpdateVariableNumberSlider();
        }

        private void ControlSliderPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //  update values upon pressing down the slider.
            UpdateVariableNumberSlider();
        }

        private void ControlSliderPreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdateVariableNumberSlider();
        }

        private void UpdateVariableNumberSlider()
        {
            var s = dynamoViewModel.Model.Scheduler;

            var t = new DelegateBasedAsyncTask(s, () =>
            {
                theModel.ComputeOutput(dynamoModel.EngineController);
            });

            t.ThenSend((_) =>
            {
            }, syncContext);

            s.ScheduleForExecution(t);
        }

        public void Dispose()
        {
            theControl.slider.PreviewMouseUp -= ControlSliderPreviewMouseUp;
            theControl.slider.PreviewMouseDown -= ControlSliderPreviewMouseDown;
            theControl.slider.ValueChanged -= ControlSliderValueChanged;
        }
    }
}
