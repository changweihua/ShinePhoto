using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ShinePhoto.UC
{
    public class AnimatedUserControl : UserControl
    {
        private bool widthAnimating;
        private SplineDoubleKeyFrame sizeAnimationWidthKeyFrame;
        private SplineDoubleKeyFrame sizeAnimationHeightKeyFrame;
        private SplineDoubleKeyFrame positionAnimationXKeyFrame;
        private SplineDoubleKeyFrame positionAnimationYKeyFrame;
        private Rectangle animatedElement;
        private DispatcherTimer timer;
        private Storyboard sizeAnimation;
        private Storyboard positionAnimation;
        private bool sizeAnimating;
        private bool positionAnimating;
        private TimeSpan sizeAnimationTimespan = new TimeSpan(0, 0, 0, 0, 500);
        private TimeSpan positionAnimationTimespan = new TimeSpan(0, 0, 0, 0, 500);
        public TimeSpan SizeAnimationDuration
        {
            get
            {
                return this.sizeAnimationTimespan;
            }
            set
            {
                this.sizeAnimationTimespan = value;
                if (this.sizeAnimationWidthKeyFrame != null)
                {
                    this.sizeAnimationWidthKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.sizeAnimationTimespan);
                }
                if (this.sizeAnimationHeightKeyFrame != null)
                {
                    this.sizeAnimationHeightKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.sizeAnimationTimespan);
                }
            }
        }
        public TimeSpan PositionAnimationDuration
        {
            get
            {
                return this.positionAnimationTimespan;
            }
            set
            {
                this.positionAnimationTimespan = value;
                if (this.positionAnimationXKeyFrame != null)
                {
                    this.positionAnimationXKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.positionAnimationTimespan);
                }
                if (this.positionAnimationYKeyFrame != null)
                {
                    this.positionAnimationYKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.positionAnimationTimespan);
                }
            }
        }
        public AnimatedUserControl()
        {
            string text = "";
            text += "<Canvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>";
            text += "  <Canvas.Resources>";
            text += "      <Storyboard x:Key='sizeStoryboard' BeginTime='00:00:00'>";
            text += "          <DoubleAnimationUsingKeyFrames Storyboard.TargetName='animatedElement' Storyboard.TargetProperty='(FrameworkElement.Width)'> ";
            text += "              <SplineDoubleKeyFrame Value='0' KeyTime='00:00:0.5' KeySpline='0.528,0,0.142,0.847' />";
            text += "           </DoubleAnimationUsingKeyFrames>";
            text += "          <DoubleAnimationUsingKeyFrames Storyboard.TargetName='animatedElement' Storyboard.TargetProperty='(FrameworkElement.Height)'> ";
            text += "             <SplineDoubleKeyFrame Value='0' KeyTime='00:00:0.5' KeySpline='0.528,0,0.142,0.847' />";
            text += "          </DoubleAnimationUsingKeyFrames>";
            text += "      </Storyboard>";
            text += "      <Storyboard x:Key='positionStoryboard' BeginTime='00:00:00'>";
            text += "          <DoubleAnimationUsingKeyFrames Storyboard.TargetName='animatedElement' Storyboard.TargetProperty='(Canvas.Left)'> ";
            text += "              <SplineDoubleKeyFrame Value='0' KeyTime='00:00:0.5' KeySpline='0.528,0,0.142,0.847' />";
            text += "          </DoubleAnimationUsingKeyFrames>";
            text += "          <DoubleAnimationUsingKeyFrames Storyboard.TargetName='animatedElement' Storyboard.TargetProperty='(Canvas.Top)'> ";
            text += "              <SplineDoubleKeyFrame Value='0' KeyTime='00:00:0.5' KeySpline='0.528,0,0.142,0.847' />";
            text += "          </DoubleAnimationUsingKeyFrames>";
            text += "      </Storyboard>";
            text += "  </Canvas.Resources>";
            text += "  <Rectangle x:Name='animatedElement' Height='0' Width='0' Canvas.Top='0' Canvas.Left='0' />";
            text += "</Canvas>";
            Canvas canvas = XamlReader.Parse(text) as Canvas;
            this.animatedElement = (canvas.Children[0] as Rectangle);
            this.sizeAnimation = (canvas.Resources["sizeStoryboard"] as Storyboard);
            this.sizeAnimation.Completed += this.SizeAnimation_Completed;
            this.positionAnimation = (canvas.Resources["positionStoryboard"] as Storyboard);
            this.positionAnimation.Completed += this.PositionAnimation_Completed;
            this.sizeAnimationWidthKeyFrame = (((DoubleAnimationUsingKeyFrames)this.sizeAnimation.Children[0]).KeyFrames[0] as SplineDoubleKeyFrame);
            this.sizeAnimationHeightKeyFrame = (((DoubleAnimationUsingKeyFrames)this.sizeAnimation.Children[1]).KeyFrames[0] as SplineDoubleKeyFrame);
            this.positionAnimationXKeyFrame = (((DoubleAnimationUsingKeyFrames)this.positionAnimation.Children[0]).KeyFrames[0] as SplineDoubleKeyFrame);
            this.positionAnimationYKeyFrame = (((DoubleAnimationUsingKeyFrames)this.positionAnimation.Children[1]).KeyFrames[0] as SplineDoubleKeyFrame);
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
        }
        public void AnimateWidth(double width)
        {
            this.widthAnimating = true;
            if (this.animatedElement != null)
            {
                if (this.sizeAnimating)
                {
                    this.sizeAnimation.Pause();
                }
                this.animatedElement.Width = ActualWidth;
                if (base.Parent != null)
                {
                    this.sizeAnimating = true;
                    this.sizeAnimationWidthKeyFrame.Value = width;
                    this.sizeAnimation.Begin();
                    this.timer.Start();
                }
                else
                {
                    base.Width = width;
                }
            }
            else
            {
                base.Width = width;
            }
        }
        public void AnimateSize(double width, double height)
        {
            this.widthAnimating = false;
            if (this.animatedElement != null)
            {
                if (this.sizeAnimating)
                {
                    this.sizeAnimation.Pause();
                }
                this.animatedElement.Width = ActualWidth;
                this.animatedElement.Height = ActualHeight;
                if (base.Parent != null)
                {
                    this.sizeAnimating = true;
                    this.sizeAnimationWidthKeyFrame.Value = width;
                    this.sizeAnimationHeightKeyFrame.Value = height;
                    this.sizeAnimation.Begin();
                    this.timer.Start();
                }
                else
                {
                    base.Width = width;
                    base.Height = height;
                }
            }
            else
            {
                base.Width = width;
                base.Height = height;
            }
        }
        public void AnimatePosition(double positionX, double positionY)
        {
            if (this.animatedElement != null)
            {
                if (this.positionAnimating)
                {
                    this.positionAnimation.Pause();
                }
                Canvas.SetLeft(this.animatedElement, Canvas.GetLeft(this));
                Canvas.SetTop(this.animatedElement, Canvas.GetTop(this));
                if (base.Parent != null)
                {
                    this.positionAnimating = true;
                    this.positionAnimationXKeyFrame.Value = positionX;
                    this.positionAnimationYKeyFrame.Value = positionY;
                    this.positionAnimation.Begin();
                    this.timer.Start();
                }
                else
                {
                    Canvas.SetLeft(this, positionX);
                    Canvas.SetTop(this, positionY);
                }
            }
            else
            {
                Canvas.SetLeft(this, positionX);
                Canvas.SetTop(this, positionY);
            }
        }
        public virtual void SizeAnimationCompleted()
        {
        }
        public virtual void PositionAnimationCompleted()
        {
        }
        private void SizeAnimation_Completed(object sender, EventArgs e)
        {
            this.sizeAnimating = false;
            base.Width = this.animatedElement.Width;
            if (!this.widthAnimating)
            {
                base.Height = this.animatedElement.Height;
            }
            else
            {
                this.widthAnimating = false;
            }
            if (!this.sizeAnimating && !this.positionAnimating)
            {
                this.timer.Stop();
            }
            this.SizeAnimationCompleted();
        }
        private void PositionAnimation_Completed(object sender, EventArgs e)
        {
            this.positionAnimating = false;
            Canvas.SetLeft(this, Canvas.GetLeft(this.animatedElement));
            Canvas.SetTop(this, Canvas.GetTop(this.animatedElement));
            if (!this.sizeAnimating && !this.positionAnimating)
            {
                this.timer.Stop();
            }
            this.PositionAnimationCompleted();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.sizeAnimating)
            {
                base.Width = this.animatedElement.Width;
                if (!this.widthAnimating)
                {
                    base.Height = this.animatedElement.Height;
                }
            }
            if (this.positionAnimating)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this.animatedElement));
                Canvas.SetTop(this, Canvas.GetTop(this.animatedElement));
            }
        }
    }
}
