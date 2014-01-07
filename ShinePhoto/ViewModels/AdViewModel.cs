using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(AdViewModel))]
    public class AdViewModel : Screen
    {
        protected override void OnInitialize()
        {
            var win = (GetView() as Window);
            LogManager.GetLog(typeof(AdViewModel)).Info("OnInitialize是否取到窗体 {0}", win != null);
            base.OnInitialize();
        }

        protected override void OnViewAttached(object view, object context)
        {
            var win = (GetView() as Window);
            LogManager.GetLog(typeof(AdViewModel)).Info("OnViewAttached是否取到窗体 {0}", win != null);
            base.OnViewAttached(view, context);
        }

        protected override void OnViewLoaded(object view)
        {
            var win = (GetView() as Window);
            LogManager.GetLog(typeof(AdViewModel)).Info("OnViewLoaded是否取到窗体 {0}", win != null);
           
            base.OnViewLoaded(view);
        }


        protected override void OnActivate()
        {
            var win = (GetView() as Window);
            LogManager.GetLog(typeof(AdViewModel)).Info("OnActivate是否取到窗体 {0}", win != null);
           
            base.OnActivate();
        }

        public void LoadAdv(object source)
        {
            var view = source as ShinePhoto.Views.AdView;
            
            Grid gridAdvert = null;
            if (view != null)
            {
                gridAdvert = ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<Grid>(view, "GridAdvert");
            }
            if (gridAdvert != null)
                FadeSB(gridAdvert.Children);//GridAdvert为界面Grid
        }

        //[ImportingConstructor]
        public AdViewModel()
        {
           
        }

        static int CurrentIndex = 0;

        private void FadeSB(UIElementCollection UIElementList)
        {
            LogManager.GetLog(typeof(AdViewModel)).Info("当前索引 {0}", CurrentIndex);
            Storyboard FadeStoryboard = new Storyboard();
            DoubleAnimation doubleAnimationX = new DoubleAnimation();
            //设置动画延时时间
            doubleAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(3000));

            doubleAnimationX.From = 1;//动画开始值
            doubleAnimationX.To = 0.1;//动画结束值
            UIElementList[CurrentIndex].Visibility = System.Windows.Visibility.Visible;
            Storyboard.SetTarget(doubleAnimationX, UIElementList[CurrentIndex]);
            Storyboard.SetTargetProperty(doubleAnimationX, new PropertyPath("Opacity"));

            FadeStoryboard.Children.Add(doubleAnimationX);
            doubleAnimationX.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseIn, };

            FadeStoryboard.Completed += ((Section, es) =>
            {
                UIElementList[CurrentIndex].Visibility = System.Windows.Visibility.Collapsed;
                CurrentIndex++;
                if (CurrentIndex == UIElementList.Count)
                {
                    CurrentIndex = 0;
                }
                //MessageBlock.Text = "当前广告:广告" + (CurrentIndex + 1);
                FadeSB(UIElementList);
            });
            FadeStoryboard.Begin();//注意WPF的begin()要放结束事件后面,不然事件不会被触发,silverlight可以放事件上面
        }
    }
}
