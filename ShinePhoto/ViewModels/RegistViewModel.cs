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

namespace ShinePhoto.ViewModels
{
    [Export(typeof(RegistViewModel))]
    public class RegistViewModel : Conductor<object>.Collection.OneActive
    {
        private UserModel _user;

        public RegistViewModel()
        {
            ActivateItem(new UserInfoViewModel());
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


        public void Next()
        {
            //得到的是窗体
            var win = GetView() as Window;
            var sp1 = FindVisualChildByName<StackPanel>(win, "sp1");
            var sp2 = FindVisualChildByName<StackPanel>(win, "sp2");
            ActivateItem(new UserInfoViewModel());
        }

        public bool CanNext
        {
            get
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword))
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region 属性

        private string _userName;
        private string _password;
        private string _rePassword;
        private string _mainBackground;
        private string _folder;
        private string _waterMarkImage;
        private string _logo;

        public string UserName {
            get { return _userName; }
            set {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public string RePassword
        {
            get { return _rePassword; }
            set
            {
                _rePassword = value;
                NotifyOfPropertyChange(() => RePassword);
                NotifyOfPropertyChange(() => CanNext);
            }
        }

        public string MainBackground
        {
            get { return _mainBackground; }
            set
            {
                _mainBackground = value;
                NotifyOfPropertyChange(() => MainBackground);
                NotifyOfPropertyChange(() => CanRegist);
            }
        }

        public string Folder
        {
            get { return _folder; }
            set
            {
                _folder = value;
                NotifyOfPropertyChange(() => Folder);
                NotifyOfPropertyChange(() => CanRegist);
            }
        }

        public string WaterMarkImage
        {
            get { return _waterMarkImage; }
            set
            {
                _waterMarkImage = value;
                NotifyOfPropertyChange(() => WaterMarkImage);
                NotifyOfPropertyChange(() => CanRegist);
            }
        }

        public string Logo
        {
            get { return _logo; }
            set
            {
                _logo = value;
                NotifyOfPropertyChange(() => Logo);
                NotifyOfPropertyChange(() => CanRegist);
            }
        }

        #endregion

        #region 方法

        public void Regist()
        { }

        public bool CanRegist
        {
            get {
                return CheckValid();
            }
        }

        public bool CheckValid()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword) || string.IsNullOrEmpty(MainBackground) || string.IsNullOrEmpty(Folder) || string.IsNullOrEmpty(WaterMarkImage) || string.IsNullOrEmpty(Logo))
            {
                return false;
            }

            return true;
        }


        #endregion

    }
}
