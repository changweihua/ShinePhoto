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
using System.Collections.ObjectModel;
using System.Windows.Ink;

using ShinePhoto.Extensions;
using System.Windows.Media;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 签名相册视图 ViewModel
    /// </summary>
    [Export(typeof(SignatureAlbumViewModel))]
    public class SignatureAlbumViewModel : Screen,IShellView
    {
        public BindableCollection<FolderModel> Folders { get; private set; }
        public BindableCollection<FileModel> ImageItems { get; private set; }

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
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
            {
            }));
        }

        #endregion

        #region 底部工具栏

        public void ShutDown()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region 侧边栏

        public void InkCanvasGesture(object evt)
        {
            var e = evt as InkCanvasGestureEventArgs;
            if (e != null)
            {
                ReadOnlyCollection<GestureRecognitionResult> gestureResults =
        e.GetGestureRecognitionResults();

                if (gestureResults[0].RecognitionConfidence ==  RecognitionConfidence.Strong)
                {
                    LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info(gestureResults[0].ApplicationGesture.GetDescriptionByName<ApplicationGesture>());
                    switch (gestureResults[0].ApplicationGesture)
                    {
                        case ApplicationGesture.AllGestures:
                            //识别所有特定于应用程序的笔势
                            break;
                        case ApplicationGesture.ArrowDown:
                            break;
                        case ApplicationGesture.ArrowLeft:
                            break;
                        case ApplicationGesture.ArrowRight:
                            break;
                        case ApplicationGesture.ArrowUp:
                            break;
                        case ApplicationGesture.Check:
                            //上行笔画的长度必须为较短的下行笔画的两倍
                            break;
                        case ApplicationGesture.ChevronDown:
                            break;
                        case ApplicationGesture.ChevronLeft:
                            break;
                        case ApplicationGesture.ChevronRight:
                            break;
                        case ApplicationGesture.ChevronUp:
                            break;
                        case ApplicationGesture.Circle:
                            LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("Circle");
                            break;
                        case ApplicationGesture.Curlicue:
                            break;
                        case ApplicationGesture.DoubleCircle:
                            break;
                        case ApplicationGesture.DoubleCurlicue:
                            break;
                        case ApplicationGesture.DoubleTap:
                            break;
                        case ApplicationGesture.Down:
                            break;
                        case ApplicationGesture.DownLeft:
                            break;
                        case ApplicationGesture.DownLeftLong:
                            break;
                        case ApplicationGesture.DownRight:
                            break;
                        case ApplicationGesture.DownRightLong:
                            break;
                        case ApplicationGesture.DownUp:
                            break;
                        case ApplicationGesture.Exclamation:
                            break;
                        case ApplicationGesture.Left:
                            break;
                        case ApplicationGesture.LeftDown:
                            break;
                        case ApplicationGesture.LeftRight:
                            break;
                        case ApplicationGesture.LeftUp:
                            break;
                        case ApplicationGesture.NoGesture:
                            //不识别任何特定于应用程序的笔势
                            break;
                        case ApplicationGesture.Right:
                            break;
                        case ApplicationGesture.RightDown:
                            break;
                        case ApplicationGesture.RightLeft:
                            break;
                        case ApplicationGesture.RightUp:
                            break;
                        case ApplicationGesture.ScratchOut:
                            //只可使用一个笔画绘制此笔势，并且该笔画至少包含三次来回移动
                            break;
                        case ApplicationGesture.SemicircleLeft:
                            break;
                        case ApplicationGesture.SemicircleRight:
                            break;
                        case ApplicationGesture.Square:
                            //可以使用一个或两个笔画绘制正方形。如果只有一个笔画，请绘制整个正方形而不抬笔。如有两个笔画，请首先绘制正方形的三条边，然后使用另一个笔画绘制剩下的一条边。绘制正方形的笔画不可超过两个。
                            break;
                        case ApplicationGesture.Star:
                            //星形必须恰有五个点，并且必须一个笔画绘制完毕而不抬笔
                            break;
                        case ApplicationGesture.Tap:
                            break;
                        case ApplicationGesture.Triangle:
                            //只可使用一个笔画绘制三角形，而不可抬笔
                            break;
                        case ApplicationGesture.Up:
                            break;
                        case ApplicationGesture.UpDown:
                            break;
                        case ApplicationGesture.UpLeft:
                            break;
                        case ApplicationGesture.UpLeftLong:
                            break;
                        case ApplicationGesture.UpRight:
                            break;
                        case ApplicationGesture.UpRightLong:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region UIElement 事件

        private int _clickCount = 0;

        public void ShowImage(object source, object container)
        {
            _clickCount += 1;

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);

            timer.Tick += (s, e1) => { timer.IsEnabled = false; _clickCount = 0; };

            timer.IsEnabled = true;

            if (_clickCount % 2 == 0)
            {
                timer.IsEnabled = false;
                _clickCount = 0;
                LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("图片被双击了");
                //(new ShinePhoto.UC.ImageViewUserControl()).
                var canvas = container as Canvas;
                var image = source as Image;
                if (canvas != null && image != null)
                {
                    canvas.Children.Add(CreateImage(image.Tag.ToString()));
                }
            }
        }

        public void OpenFolder(object origin, object evt, object container)
        {

            var button = origin as Button;
            var e = evt as MouseButtonEventArgs;
            var ic = container as ListView;
            //var grid = obj as Grid;
            //var loading = load as ShinePhoto.UC.LoadingUserControl;
            if (button != null && ic != null)
            {
                long size = 0;
                string folderName = button.Tag.ToString();

                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
                {
                    ImageItems = new BindableCollection<FileModel>();
                    var arr = System.IO.Directory.GetFiles(folderName);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        ImageItems.Add(new FileModel { FileName = arr[i], Height = 158, Width = 282 });
                        //ic.Items.Add(new FileModel { FileName = arr[i], Height = 158, Width = 282 });
                    }
                    NotifyOfPropertyChange(() => ImageItems);
                }));

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
                    files = lfd.EndInvoke(asyncResult);
                    //ic.ItemsSource = files;
                    //ic.Items.Clear();

                    //p.Children.Add(new Image { Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(item.FileName, UriKind.RelativeOrAbsolute)), Width = 100 });
                    //Dispatcher.CurrentDispatcher.BeginInvoke(new System.Action(() =>
                    //{
                    //    foreach (var item in files)
                    //    {
                    //        ic.Items.Add(new Image { Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(item.FileName, UriKind.RelativeOrAbsolute)), Margin = new Thickness(5), Width = 200, Height = 150 });
                    //    }
                    //}));
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

        #region 图片展示

        public void ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("ManipulationInertiaStarting");
            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0)
            };

            e.ExpansionBehavior = new InertiaExpansionBehavior()
            {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity,
                DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0
            };

            e.RotationBehavior = new InertiaRotationBehavior()
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = 720 / (1000.0 * 1000.0)
            };
            e.Handled = true;
        }

        public void ManipulationStarting(object container, ManipulationStartingEventArgs e)
        {
            LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("ManipulationStarting");
            var uie = e.OriginalSource as UIElement;
            if (uie != null)
            {
                if (last != null) Canvas.SetZIndex(last, 0);
                Canvas.SetZIndex(uie, 2);
                last = uie;
            }

            e.ManipulationContainer = (container as Canvas);
            e.Handled = true;
        }

        public void ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info("ManipulationDelta");
            try
            {
                var element = e.Source as FrameworkElement;
                if (element != null)
                {
                    var deltaManipulation = e.DeltaManipulation;
                    var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
                    //var matrix = ((MatrixTransform)(element.RenderTransform.Clone())).Matrix;
                    // find the old center; arguaby this could be cached 
                    Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
                    // transform it to take into account transforms from previous manipulations 
                    center = matrix.Transform(center);
                    //this will be a Zoom. 
                    matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                    // Rotation 
                    matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
                    //Translation (pan) 
                    matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
                    
                    ((MatrixTransform)element.RenderTransform).Matrix = matrix;

                    e.Handled = true;

                    if (e.IsInertial)
                    {
                        Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

                        Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));

                        if (e.IsInertial && !containingRect.Contains(shapeBounds))
                        {
                            //Report that we have gone over our boundary 
                            e.ReportBoundaryFeedback(e.DeltaManipulation);

                            e.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLog(typeof(SignatureAlbumViewModel)).Info(ex.Message);
            }
            
        }

        #endregion

        #region 方法

        /// <summary>
        /// 创建 Image 控件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Image CreateImage(string path)
        {
            Image image = new Image();
            //MatrixTransform matrixTransform = new MatrixTransform(new Matrix(1.5929750047527, 0.585411309251951, -0.585411309251951, 1.5929750047527, 564.691807426081, 79.4658072348299));
            //image.RenderTransform = matrixTransform;
            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            image.Width = 300;
            image.Height = 200;
            image.IsManipulationEnabled = true;
            image.SetValue(Canvas.RightProperty, 1920.0 / 2);
            image.SetValue(Canvas.BottomProperty, 1080.0 / 2);
            image.RenderTransform = new MatrixTransform();

            return image;
        }

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

        UIElement last;

        private Dictionary<GestureRecognitionResult, string> _dict = new Dictionary<GestureRecognitionResult, string>();
        private long _folderSize = 0;

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
