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
using System.Data.SQLite;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 注册视图 ViewModel
    /// </summary>
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
            User = new UserModel();
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

        public void Back()
        {
            var win = GetView() as Window;
            win.Close();
        }

        public void Prev()
        {
            //得到的是窗体
            var win = GetView() as Window;
            var sp1 = FindVisualChildByName<StackPanel>(win, "sp1");
            var sp2 = FindVisualChildByName<StackPanel>(win, "sp2");
            sp2.Visibility = Visibility.Collapsed;
            sp1.Visibility = Visibility.Visible;
            this.ChangeActiveItem(new UserInfoViewModel(_eventAggregator), false);
        }

        public bool CanPrev
        {
            get
            {
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
            this.ChangeActiveItem(new SettingViewModel(_eventAggregator), false);
        }

        public bool CanFinish
        {
            get;
            private set;
        }

        public void Finish()
        {
            User.CreateDate = DateTime.Now.ToString();
            User.UserId = Guid.NewGuid().ToString();
            User.Password = ShinePhoto.Helpers.MD5Helper.GetMD5StringLowerCase(User.Password);
            using (ShinePhotoDataContext ctx = new ShinePhotoDataContext(new SQLiteConnection(@"data source=D:\db.sl3")))
            {
                ctx.Users.InsertOnSubmit(User);
                ctx.SubmitChanges();
            }
        }

        public bool CanNext
        {
            get;
            private set;
        }

        public void Handle(object message)
        {
            if (message is UserInfoEvent)
            {
                System.Diagnostics.Debug.WriteLine("已经监听到了" + (message as UserInfoEvent).CanGo);

                var evt = message as UserInfoEvent;

                if (evt != null && evt.CanGo)
                {
                    CanNext = true;
                    User.UserName = evt.UserInfo.UserName;
                    User.Password = evt.UserInfo.Password;
                    NotifyOfPropertyChange(() => CanNext);
                }
            }
            if (message is SettingEvent)
            {
                var evt = message as SettingEvent;

                if (evt != null && evt.CanGo)
                {
                    CanFinish = true;
                    User.MainBackground = evt.Setting.MainBackground;
                    User.Folder = evt.Setting.Folder;
                    User.Logo = evt.Setting.Logo;
                    User.WaterMarkImage = evt.Setting.WaterMarkImage;
                    NotifyOfPropertyChange(() => CanFinish);
                }
            }
        }

        #endregion
 
        #region 方法

        #endregion

    }
}
