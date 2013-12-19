using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using ShinePhoto.Models;
using ShinePhoto.Events;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(RegistViewModel))]
    public class RegistViewModel : Conductor<object>, IHandle<object> 
    {

        //public RegistViewModel()
        //{
        //    ActivateItem(new UserInfoViewModel());
        //}
        public UserModel User { get; private set; }
        private IEventAggregator _eventAggregator;

        public RegistViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            ActivateItem(new UserInfoViewModel(eventAggregator));
        }

        public void OperationButtonClick(object source, object sender)
        {
            var button = source as Button;
            var vm = sender as RegistViewModel;
            //得到的是窗体
            var win = GetView() as Window;
            var sp1 = FindVisualChildByName<StackPanel>(win, "sp1");
            var sp2 = FindVisualChildByName<StackPanel>(win, "sp2");

            //ActiveItem
            if (source != null)
            {
                switch (button.Tag.ToString())
                {
                    case "NEXT":
                        sp2.Visibility = Visibility.Visible;
                        sp1.Visibility = Visibility.Collapsed;
                        break;
                    case "PREV":
                        sp1.Visibility = Visibility.Visible;
                        sp2.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
        }

        public T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        #region 标题栏操作

        public void MoveWindow(object sender, MouseButtonEventArgs e, object view)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                (view as Window).DragMove();
            }
        }

        public void CloseWindow(object view)
        {
            if ((view as Window) != null)
            {
                (view as Window).Close();
            }
        }

        #endregion

        #region 底部按钮操作

        public void Prev()
        {
            //得到的是窗体
            var win = GetView() as Window;
            var sp1 = FindVisualChildByName<StackPanel>(win, "sp1");
            var sp2 = FindVisualChildByName<StackPanel>(win, "sp2");
            sp2.Visibility = Visibility.Collapsed;
            sp1.Visibility = Visibility.Visible;
            ActivateItem(new UserInfoViewModel(_eventAggregator));
        }

        public bool CanPrev
        {
            get
            {
                //if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword))
                //{
                //    return false;
                //}

                return true;
            }
        }

        public void Next()
        {
            //得到的是窗体
            var win = GetView() as Window;
            var sp1 = FindVisualChildByName<StackPanel>(win, "sp1");
            var sp2 = FindVisualChildByName<StackPanel>(win, "sp2");
            sp1.Visibility = Visibility.Collapsed;
            sp2.Visibility = Visibility.Visible;
            ActivateItem(new SettingViewModel(_eventAggregator));
        }

        public bool CanNext
        {
            get;
            private set;
        }

        public void Handle(object message)
        {
            if (message is RegistNextEvent)
            {
                User = (message as RegistNextEvent).User;
                System.Diagnostics.Debug.WriteLine("已经监听到了" + (message as RegistNextEvent).CanGo);
                if((message as RegistNextEvent).CanGo)
                {
                    CanNext = true;
                    NotifyOfPropertyChange(() => CanNext); 
                }
            }
        }

        #endregion
 
        #region 方法

        public void Regist()
        { }

        public bool CanRegist
        {
            get;
            private set;
        }


        #endregion

    }
}
