using System.ComponentModel;
using System.Windows.Controls;

namespace CeleryX.Nodes.InputBoundedNumberSlider
{
    /// <summary>
    /// Interaction logic for InputBoundedNumberSliderControl.xaml
    /// </summary>
    public partial class InputBoundedNumberSliderControl : UserControl, INotifyPropertyChanged
    {
        public InputBoundedNumberSliderControl(InputBoundedNumberSliderNodeModel model)
        {
            InitializeComponent();

            //  2022.03.31 New Addition
            model.PropertyChanged += Model_PropertyChanged;

            DataContext = this;
            //
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //  If there are some internal values you want to change in the UI, do it here.
            if (e.PropertyName == "DataUpdated")
            {
                var model = sender as InputBoundedNumberSliderNodeModel;
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
            var xadj = ActualWidth + e.HorizontalChange;

            if (this.Parent.GetType() == typeof(Grid))
            {
                var inputgrid = this.Parent as Grid;
                if(xadj >= inputgrid.MinWidth)
                {
                    Width = xadj;
                }
            }
        }
    }
}
