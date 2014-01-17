using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShinePhoto.UC
{
    public class ThumbEx : Control
    {
        private bool mouseDown;
        private Point downPosition;
        private Point lastPosition = new Point(0.0, 0.0);
        private MouseEventArgs lastEventArgs;
        public event EventHandler<DragEventArgs> DragStarted;
        public event EventHandler<DragEventArgs> DragDelta;
        public event EventHandler<DragEventArgs> DragFinished;
        public ThumbEx()
        {
            this.MouseLeftButtonUp += Thumb_MouseLeftButtonUp;
            this.MouseMove += Thumb_MouseMove;
            this.MouseLeftButtonDown += Thumb_MouseLeftButtonDown;
            //base.add_MouseLeftButtonUp(new MouseButtonEventHandler(this.Thumb_MouseLeftButtonUp));
            //base.add_MouseMove(new MouseEventHandler(this.Thumb_MouseMove));
            //base.add_MouseLeftButtonDown(new MouseButtonEventHandler(this.Thumb_MouseLeftButtonDown));
            if (this.Cursor == null)
            {
                this.Cursor = Cursors.Hand;
            }
        }
        public void AdjustDownPosition(Point difference)
        {
            if (this.mouseDown)
            {
                this.downPosition.X = this.downPosition.X + difference.X;
                this.downPosition.Y = this.downPosition.Y + difference.Y;
                if (this.DragDelta != null)
                {
                    this.DragDelta.Invoke(this, new DragEventArgs(this.lastPosition.X - this.downPosition.X, this.lastPosition.Y - this.downPosition.Y, this.lastEventArgs));
                }
            }
        }
        private void Thumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            FrameworkElement frameworkElement = sender as FrameworkElement;
            frameworkElement.CaptureMouse();
            this.downPosition = e.GetPosition(frameworkElement);
            this.mouseDown = true;
            if (this.DragStarted != null)
            {
                this.DragStarted.Invoke(this, new DragEventArgs(e.GetPosition(frameworkElement).X - this.downPosition.X, e.GetPosition(frameworkElement).Y - this.downPosition.Y, e));
            }
        }
        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if (this.mouseDown)
            {
                if (this.DragDelta != null)
                {
                    this.lastPosition = e.GetPosition(frameworkElement);
                    this.lastEventArgs = e;
                    this.DragDelta.Invoke(this, new DragEventArgs(e.GetPosition(frameworkElement).X - this.downPosition.X, e.GetPosition(frameworkElement).Y - this.downPosition.Y, e));
                }
            }
        }
        private void Thumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            frameworkElement.ReleaseMouseCapture();
            this.mouseDown = false;
            if (this.DragFinished != null)
            {
                this.DragFinished.Invoke(this, new DragEventArgs(e.GetPosition(frameworkElement).X - this.downPosition.X, e.GetPosition(frameworkElement).Y - this.downPosition.Y, e));
            }
        }
    }
}
