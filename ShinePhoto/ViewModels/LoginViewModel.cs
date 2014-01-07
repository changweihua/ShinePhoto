using System.Linq;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows.Input;
using System.Windows;
using ShinePhoto.Models;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 登录视图 ViewModel
    /// </summary>
    [Export(typeof(LoginViewModel))]
    public class LoginViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _events;

        [ImportingConstructor]
        public LoginViewModel(IWindowManager windowManager, IEventAggregator events)
        {
            user = new LoginUserModel();
            _windowManager = windowManager;
            _events = events;
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
            _windowManager.ShowWindow(new RegistViewModel(_events), null);
        }

        public void Login()
        {
            
            using (ShinePhotoDataContext ctx = new ShinePhotoDataContext(new SQLiteConnection(@"data source=D:\db.sl3")))
            {
                var user = ctx.Users.Where(_ => _.UserName == UserName && _.Password == ShinePhoto.Helpers.MD5Helper.GetMD5StringLowerCase(Password)).SingleOrDefault();

                if (user != null)
                {
                    _windowManager.ShowWindow(ViewLocator.LocateTypeForModelType(typeof(MainViewModel),null,null), null, null);
                    //System.Diagnostics.Debug.WriteLine("登录成功");
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

        private LoginUserModel user = null;

        private string _userName = "";

        [Required(ErrorMessage = "用户名必须填写")]
        public string UserName
        {
            get { return _userName; }
            set
            {
                //user.UserName = value;
                //Validator.ValidateProperty(_userName, new ValidationContext(user, null, null) { MemberName = "UserName" });

                //Validator.TryValidateObject(this, new ValidationContext(this, null, null) { MemberName = "UserName" }, null, false);
                //var isValid = Validator.TryValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "UserName" }, null);

                //if (isValid)
                //{

                //}

                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "UserName" });
                _userName = value;
                
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password = "";
        [Required(ErrorMessage = "密码必须填写")]
        public string Password
        {
            get { return _password; }
            set
            {

                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Password" });

                _password = value;

                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        #endregion

        #region 公共方法

        bool CheckValid()
        {
            //var errors = new List<ValidationResult>();
            //var isValid = Validator.TryValidateObject(this, new ValidationContext(this, null, null), errors, true);

            //if (!isValid)
            //{
            //    return false;
            //    throw new AggregateException(
            //        errors.Select((e) => new ValidationException(e.ErrorMessage)
            //    ));
            //}
           
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return false;
            }
            
            return true;
        }

        #endregion


    }
}
