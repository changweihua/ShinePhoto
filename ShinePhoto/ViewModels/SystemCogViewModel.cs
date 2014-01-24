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
        //private BindableCollection<string> _categoryItems;

        //public BindableCollection<string> CategoryItems {
        //    get {
        //        return _categoryItems;
        //    }
        //    set {
        //        _categoryItems = value;
        //        NotifyOfPropertyChange(() => CategoryItems);
        //    }
        //}

        public BindableCollection<string> CategoryItems { get; private set; }

        public SystemCogViewModel()
        {
            if (Execute.InDesignMode)
                LoadDesignData();
            
        }

        private void LoadDesignData()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
            {
                CategoryItems = new BindableCollection<string>();
                var arr = Enumerable.Range(1, 5).ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    CategoryItems.Add(arr[i].ToString());
                }
            }));
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
