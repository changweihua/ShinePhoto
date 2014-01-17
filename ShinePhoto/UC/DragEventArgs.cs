using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ShinePhoto.UC
{
    public class DragEventArgs : EventArgs
    {
        public double HorizontalChange
        {
            get;
            set;
        }
        public double VerticalChange
        {
            get;
            set;
        }
        public MouseEventArgs MouseEventArgs
        {
            get;
            set;
        }
        public DragEventArgs()
        {
        }
        public DragEventArgs(double horizontalChange, double verticalChange, MouseEventArgs mouseEventArgs)
        {
            this.HorizontalChange = horizontalChange;
            this.VerticalChange = verticalChange;
            this.MouseEventArgs = mouseEventArgs;
        }
    }
}
