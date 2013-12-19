using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using ShinePhoto.Events;
using ShinePhoto.Models;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(UserInfoViewModel))]
    public class UserInfoViewModel : Screen
    {
        public RegistViewModel ParentViewModel{get;set;}

        protected override void OnDeactivate(bool close)
        {
            //ParentViewModel = (this.Parent as RegistViewModel);
            //UserName = ParentViewModel.User.UserName;
            base.OnDeactivate(close);
        }

        protected override void OnActivate()
        {
            ParentViewModel = (this.Parent as RegistViewModel);
            UserName = ParentViewModel.User.UserName;
            Password = ParentViewModel.User.Password;
            RePassword = ParentViewModel.User.Password;
            base.OnActivate();
        }

        #region 事件传递与获取

        private readonly IEventAggregator _eventAggregator;

        public UserInfoViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region 属性

        private string _userName;
        private string _password;
        private string _rePassword;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        public string RePassword
        {
            get { return _rePassword; }
            set
            {
                _rePassword = value;
                NotifyOfPropertyChange(() => RePassword);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        #endregion

        #region 方法

        public void Go()
        {
            System.Diagnostics.Debug.WriteLine("发布事件");
        }

        public bool CanGo
        {
            get
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword) || string.Compare(Password, RePassword) != 0)
                {
                    _eventAggregator.Publish(new UserInfoEvent(new UserInfo { }, false));
                    return false;
                }
                _eventAggregator.Publish(new UserInfoEvent(new UserInfo { UserName = UserName, Password = Password }, true));
                return true;
            }
        }

        #endregion

    }
}
