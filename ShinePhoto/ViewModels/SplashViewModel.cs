using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(SplashViewModel))]
    public class SplashViewModel : Screen
    {
        public void Success()
        {
            var win = GetView() as Window;


            //var mainViewModel = IoC.Get<MainViewModel>();
            //var winManager = IoC.Get<IWindowManager>();

            //winManager.ShowWindow(mainViewModel);

            //var mainView = (mainViewModel as Screen).GetView() as Window;
            //Application.Current.MainWindow = mainView;

            Application.Current.MainWindow = null;
            win.DialogResult = true;
            win.Close();
        }
    }
}
