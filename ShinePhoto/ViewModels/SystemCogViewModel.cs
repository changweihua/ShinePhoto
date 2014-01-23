using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using ShinePhoto.Interface;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Input;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(SystemCogViewModel))]
    public class SystemCogViewModel : Screen
    {
        public SystemCogViewModel()
        {
            //if (Execute.InDesignMode)
                //LoadDesignData();

            //LoadDesignData();
        }

        private void LoadDesignData()
        {
            var rd1 = System.Windows.Application.LoadComponent(new Uri(@"Styles\Metro\Dark.xaml", UriKind.Relative)) as System.Windows.ResourceDictionary;
            var rd3 = System.Windows.Application.LoadComponent(new Uri(@"Styles\Metro\MetroControls.xaml", UriKind.Relative)) as System.Windows.ResourceDictionary;
            var rd2 = System.Windows.Application.LoadComponent(new Uri(@"Styles\Metro\OtherMetroTabs.xaml", UriKind.Relative)) as System.Windows.ResourceDictionary;
            if (rd1 != null && rd2 != null)
            {
                LogManager.GetLog(typeof(SystemCogViewModel)).Info("成功加载");
                Application.Current.Resources.MergedDictionaries.Add(rd1);
                Application.Current.Resources.MergedDictionaries.Add(rd2);
                Application.Current.Resources.MergedDictionaries.Add(rd3);
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

        #region Properties

        private string _title = "程序设置";

        public string WindowTitle
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }


        #endregion

    }
}
