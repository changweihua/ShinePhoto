using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows.Input;
using System.Windows;

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

        #endregion

    }
}
