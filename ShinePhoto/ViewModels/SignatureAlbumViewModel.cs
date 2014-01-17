using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Interface;
using ShinePhoto.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using System.IO;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 签名相册视图 ViewModel
    /// </summary>
    [Export(typeof(SignatureAlbumViewModel))]
    public class SignatureAlbumViewModel : Screen,IShellView
    {
        public BindableCollection<FolderModel> Folders { get; private set; }

        private readonly string RootFolder = @"C:\Users\ChangWeihua\Pictures\Eye-Fi";

        #region UserControl 方法

        public void LoadUserControl(object source)
        {
            var view = source as ShinePhoto.Views.SignatureAlbumView;

            if (view != null)
            {
                string pattern = string.Format("{0}-{1}-*", DateTime.Parse(view.datePicker.CurrentDate).Year, DateTime.Parse(view.datePicker.CurrentDate).Month);
                Folders = new BindableCollection<FolderModel>(LoadDirectories(RootFolder, pattern));
                NotifyOfPropertyChange(() => Folders);
                //view.Folders.ItemsSource = Folders;
            }

        }

        #endregion

        #region UIElement 事件

        public void OpenFolder(object origin, object evt, object container)
        {

            var button = origin as Button;
            var e = evt as MouseButtonEventArgs;
            var ic = container as ItemsControl;
            //var grid = obj as Grid;
            //var loading = load as ShinePhoto.UC.LoadingUserControl;
            if (button != null && ic != null)
            {
               
                
                ic.Visibility = Visibility.Collapsed;
                long size = 0;
                string folderName = button.Tag.ToString();
                CalculateFolderSizeDelegate cfsd = CalculateFolderSize;
                //异步回调
                cfsd.BeginInvoke(folderName, (result) =>
                {
                    try
                    {
                        size = cfsd.EndInvoke(result);

                        LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("\n计算完成。文件夹 {0} 大小为: {1} M", (string)result.AsyncState, size / 1024.0 / 1024.0);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("发生异常，异常信息: {0}", ex.Message);
                    }

                }, folderName);

                LoadFilesDelegate lfd = LoadFiles;
                List<FileModel> files = new List<FileModel>();

                //在UI线程中得到异步任务的返回值，并更新UI
                //必须在UI线程中执行 
                Action<IAsyncResult> resultHandler = delegate(IAsyncResult asyncResult)
                {
                    ic.Visibility = Visibility.Visible;
                    files = lfd.EndInvoke(asyncResult);
                    ic.Items.Clear();

                    foreach (var item in files)
                    {
                        ic.Items.Add(new Image { Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(item.FileName, UriKind.RelativeOrAbsolute)), Width = 100 });
                    }
                    LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("\n遍历 {0} 完成，总计 {1} 个文件", (string)asyncResult.AsyncState, files.Count);
                };

                lfd.BeginInvoke(folderName, "", (result) =>
                {
                    try
                    {
                        (GetView() as ShinePhoto.Views.SignatureAlbumView).Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, resultHandler, result);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("发生异常，异常信息: {0}", ex.Message);
                    }

                }, folderName);

            }

        }

        //public void OpenFolder(object origin, object evt)
        //{
        //    var image = origin as Image;
        //    var e = evt as MouseButtonEventArgs;

        //    if (e != null)
        //    {
        //        _clickCount += 1;
        //        _prevPoint = e.GetPosition(image);
        //        DispatcherTimer timer = new DispatcherTimer();
        //        timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
        //        timer.Tick += (a, b) => { timer.IsEnabled = false; _clickCount = 0; _prevPoint = new Point(0, 0); };
        //        timer.IsEnabled = true;
        //        if (_clickCount % 2 == 0 && (Math.Abs(_prevPoint.X - e.GetPosition(image).X) < 5 && Math.Abs(_prevPoint.Y - e.GetPosition(image).Y) < 5))
        //        {
        //            timer.IsEnabled = false;
        //            _clickCount = 0;
        //            _prevPoint = new Point(0, 0);
        //            LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("双击");
        //        }
        //        else
        //        {
        //            LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("单击");
        //        }

        //    }

        //}

        public void Flip(object source)
        {
            var fc = source as ShinePhoto.UC.FlipUserControl;
            if (fc != null)
            {
                fc.Flip();
            }
        }

        public void SelectDate(object obj1)
        {
            var picker = obj1 as ShinePhoto.UC.HoloDatePickerUserControl;
            if (picker != null)
            {
                string pattern = string.Format("{0}-{1}-*", picker.CurrentYear, picker.CurrentMonth);
                LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info(pattern);
                Folders = new BindableCollection<FolderModel>(LoadDirectories(RootFolder, pattern));
                NotifyOfPropertyChange(() => Folders);
            }
        }

        #endregion

        #region 委托

        /// <summary>
        /// 计算文件夹大小
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public delegate long CalculateFolderSizeDelegate(string folderName);

        /// <summary>
        /// 读取文件夹下所有的图片
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public delegate List<FileModel> LoadFilesDelegate(string folderName, string pattern);

        #endregion

        #region 方法

        long CalculateFolderSize(string folderName)
        {

            DirectoryInfo di = new DirectoryInfo(folderName);
            var files = di.GetFiles();
            foreach (var file in files)
            {
                _folderSize += file.Length;
            }

            var folders = di.GetDirectories();

            foreach (var dir in folders)
            {
                CalculateFolderSize(dir.FullName);
            }


            return _folderSize;
        } 

        List<FolderModel> LoadDirectories(string parent, string pattern = "*")
        {
            var list = from folder
                       in System.IO.Directory.GetDirectories(RootFolder, pattern)
                       //where new System.IO.DirectoryInfo(folder).Attributes != (System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System | System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Encrypted)
                       //where ((new System.IO.DirectoryInfo(folder).Attributes & System.IO.FileAttributes.Normal) == System.IO.FileAttributes.Normal) //|| ((new System.IO.DirectoryInfo(folder).Attributes & System.IO.FileAttributes.ReadOnly) != System.IO.FileAttributes.ReadOnly) || ((new System.IO.DirectoryInfo(folder).Attributes & System.IO.FileAttributes.System) != System.IO.FileAttributes.System)
                       let fi = new System.IO.DirectoryInfo(folder)
                       where !(fi.Attributes.HasFlag(System.IO.FileAttributes.Hidden) || fi.Attributes.HasFlag(System.IO.FileAttributes.Archive) )
                       select new FolderModel { FolderPath = folder };

            return list.ToList();
        }

        List<FileModel> LoadFiles(string folder, string pattern = "*")
        {
            var files = from file in System.IO.Directory.GetFiles(folder) select new FileModel { FileName = file };

            return files.ToList();
        }

        #endregion

        #region 属性

        private long _folderSize = 0;

        private Point _prevPoint = new Point(0, 0);
        private DispatcherTimer _timer;
        private int _clickCount = 0;

        /// <summary>
        /// 程序品牌 Logo 高度
        /// </summary>
        private double _logoHeight = 120d;

        public double LogoHeight
        {
            get
            {
                return _logoHeight;
            }
            set
            {
                _logoHeight = value;
                NotifyOfPropertyChange(() => LogoHeight);
            }
        }

        #endregion
    }
}
