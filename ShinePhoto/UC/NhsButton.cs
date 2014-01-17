using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Globalization;

namespace ShinePhoto.UC
{
    public class NhsButton : Control
    {
        private Storyboard runningAnimation;
        private Storyboard mouseEnter;
        private Storyboard mouseLeave;
        private Storyboard mouseDown;
        private Storyboard mouseUp;
        public event EventHandler Clicked;
        public NhsButton()
        {
            this.MouseLeftButtonUp += Button_MouseLeftButtonUp;
            //base.add_MouseLeftButtonUp(new MouseButtonEventHandler(this.Button_MouseLeftButtonUp));
            if (base.Cursor == null)
            {
                base.Cursor = Cursors.Hand;
            }
            this.MouseEnter += Button_MouseEnter;
            this.MouseLeave += Button_MouseLeave;
            this.MouseLeftButtonDown += Button_MouseLeftButtonDown;
            //base.add_MouseEnter(new MouseEventHandler(this.Button_MouseEnter));
            //base.add_MouseLeave(new MouseEventHandler(this.Button_MouseLeave));
            //base.add_MouseLeftButtonDown(new MouseButtonEventHandler(this.Button_MouseLeftButtonDown));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (base.GetTemplateChild("MouseDownAnimation") != null)
            {
                this.mouseDown = (Storyboard)base.GetTemplateChild("MouseDownAnimation");
                this.mouseDown.Completed += this.Storyboard_Completed;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (base.GetTemplateChild("MouseDownAnimation" + i.ToString(CultureInfo.CurrentCulture)) != null)
                    {
                        this.mouseDown = (Storyboard)base.GetTemplateChild("MouseDownAnimation" + i.ToString(CultureInfo.CurrentCulture));
                        this.mouseDown.Completed += this.Storyboard_Completed;
                    }
                }
            }
            if (base.GetTemplateChild("MouseUpAnimation") != null)
            {
                this.mouseUp = (Storyboard)base.GetTemplateChild("MouseUpAnimation");
                this.mouseUp.Completed += this.Storyboard_Completed;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (base.GetTemplateChild("MouseUpAnimation" + i.ToString(CultureInfo.CurrentCulture)) != null)
                    {
                        this.mouseUp = (Storyboard)base.GetTemplateChild("MouseUpAnimation" + i.ToString(CultureInfo.CurrentCulture));
                        this.mouseUp.Completed += this.Storyboard_Completed;
                    }
                }
            }
            if (base.GetTemplateChild("MouseEnterAnimation") != null)
            {
                this.mouseEnter = (Storyboard)base.GetTemplateChild("MouseEnterAnimation");
                this.mouseEnter.Completed += this.Storyboard_Completed;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (base.GetTemplateChild("MouseEnterAnimation" + i.ToString(CultureInfo.CurrentCulture)) != null)
                    {
                        this.mouseEnter = (Storyboard)base.GetTemplateChild("MouseEnterAnimation" + i.ToString(CultureInfo.CurrentCulture));
                        this.mouseEnter.Completed += this.Storyboard_Completed;
                    }
                }
            }
            if (base.GetTemplateChild("MouseLeaveAnimation") != null)
            {
                this.mouseLeave = (Storyboard)base.GetTemplateChild("MouseLeaveAnimation");
                this.mouseLeave.Completed += this.Storyboard_Completed;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (base.GetTemplateChild("MouseLeaveAnimation" + i.ToString(CultureInfo.CurrentCulture)) != null)
                    {
                        this.mouseLeave = (Storyboard)base.GetTemplateChild("MouseLeaveAnimation" + i.ToString(CultureInfo.CurrentCulture));
                        this.mouseLeave.Completed += this.Storyboard_Completed;
                    }
                }
            }
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.runningAnimation = null;
        }
        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.mouseDown != null)
            {
                if (this.runningAnimation != null)
                {
                    this.runningAnimation.Stop();
                    this.runningAnimation = null;
                }
                this.runningAnimation = this.mouseDown;
                try
                {
                    this.runningAnimation.Begin();
                }
                catch (ArgumentException)
                {
                    this.runningAnimation = null;
                }
            }
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.mouseLeave != null)
            {
                if (this.runningAnimation != null)
                {
                    this.runningAnimation.Stop();
                    this.runningAnimation = null;
                }
                this.runningAnimation = this.mouseLeave;
                try
                {
                    this.runningAnimation.Begin();
                }
                catch (ArgumentException)
                {
                    this.runningAnimation = null;
                }
            }
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.mouseEnter != null)
            {
                if (this.runningAnimation != null)
                {
                    this.runningAnimation.Stop();
                    this.runningAnimation = null;
                }
                this.runningAnimation = this.mouseEnter;
                try
                {
                    this.runningAnimation.Begin();
                }
                catch (ArgumentException)
                {
                    this.runningAnimation = null;
                }
            }
        }
        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (this.mouseUp != null)
            {
                if (this.runningAnimation != null)
                {
                    this.runningAnimation.Stop();
                    this.runningAnimation = null;
                }
                this.runningAnimation = this.mouseUp;
                try
                {
                    this.runningAnimation.Begin();
                }
                catch (ArgumentException)
                {
                    this.runningAnimation = null;
                }
            }
            if (this.Clicked != null)
            {
                this.Clicked.Invoke(this, e);
            }
        }
    }
}
