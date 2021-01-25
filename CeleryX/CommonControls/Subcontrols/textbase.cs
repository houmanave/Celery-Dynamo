using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CeleryX.CommonControls.Subcontrols
{
    public class Textbase : TextBlock
    {
        private bool _isRightOriented;
        private bool _isBottomOriented;
        private string _observableText;

        public bool IsRightOriented
        {
            get
            {
                return _isRightOriented;
            }

            set
            {
                _isRightOriented = value;
            }
        }

        public bool IsBottomOriented
        {
            get
            {
                return _isBottomOriented;
            }

            set
            {
                _isBottomOriented = value;
            }
        }

        public string ObservableText
        {
            get
            {
                return _observableText;
            }

            set
            {
                _observableText = value;
                this.Text = _observableText;
            }
        }

        public Textbase()
        {

        }

        public virtual void Regenerate(Point p)
        {
        }
    }
}
