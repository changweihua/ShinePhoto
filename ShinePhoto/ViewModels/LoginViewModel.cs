using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows.Input;
using System.Windows;
using ShinePhoto.Models;
using System.Data.SQLite;
using System.Data.Linq;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(LoginViewModel))]
    public class LoginViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public LoginViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        private string _title = "登录窗体";

        public string WindowTitle
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
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

        public void ShowRegist()
        {
            _windowManager.ShowWindow(new RegistViewModel(), null);
        }

        public void Login()
        {
            using (ShinePhotoDataContext ctx = new ShinePhotoDataContext(new SQLiteConnection(@"data source=db.sl3")))
            {
                var user = ctx.Users.Where(_ => _.UserName == UserName && _.Password == ShinePhoto.Helpers.MD5Helper.GetMD5StringLowerCase(Password)).SingleOrDefault();

                if (user != null)
                { 
                    System.Diagnostics.Debug.WriteLine("登录成功");
                }
            }
        }

        public bool CanLogin
        {
            get {
                return CheckValid();
            }
        }

        #region 属性

        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        #endregion

        #region 公共方法

        bool CheckValid()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }
            
            return true;
        }

        #endregion

    }
}
