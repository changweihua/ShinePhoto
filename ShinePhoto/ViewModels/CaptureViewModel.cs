using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Models;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using ShinePhoto.Loaders;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Collections;
using System.Windows.Ink;
using System.Windows;
using Xceed.Wpf.Toolkit;
using System.Windows.Media.Animation;
using ShinePhoto.Interface;
using System.Windows.Input;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 即拍即得视图 ViewModel
    /// </summary>
    [Export(typeof(CaptureViewModel))]
    public class CaptureViewModel : Screen, IShellView
    {

        /// <summary>
        /// 图片集合
        /// </summary>
        public BindableCollection<FileModel> FileModels { get; private set; }

        /// <summary>
        /// 事件聚合器
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public CaptureViewModel(IEventAggregator eventAggregator)
        {
            //Caliburn.Micro.Coroutine.BeginExecute((new[] { new ImageLoader("") }).ToList<IResult>().GetEnumerator());
            
             _eventAggregator = eventAggregator;

             //Caliburn.Micro.Coroutine.BeginExecute(LoadData().GetEnumerator());
             //Caliburn.Micro.Coroutine.BeginExecute(LoadDataAsync().GetEnumerator());

             System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
             {
                 FileModels = new BindableCollection<FileModel>();
                 var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
                 for (int i = 0; i < arr.Length; i++)
                 {
                     FileModels.Add(new FileModel { FileName = arr[i], Height = 158, Width = 282 });
                 }
             }));

             FileSystemWatcher watcher = new FileSystemWatcher();
             watcher.Filter = "*.jpg";
             watcher.Path = @"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13";
             //watcher.Changed += new FileSystemEventHandler(OnProcess);
             watcher.Created += new FileSystemEventHandler(OnProcess);
             watcher.Deleted += new FileSystemEventHandler(OnProcess);
             watcher.Renamed += new RenamedEventHandler(OnRenamed);
             watcher.EnableRaisingEvents = true;
             watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;

        }

        #region 异步事件


        //public IEnumerable<IResult> ProcessTask()
        //{
        //    FileModel result ;

        //    for (int i = 1; i < 4; i++) // Simulates a loop that processes multiple items, files, fields...
        //    {
        //        yield return new BackgroundCoRoutine(() =>
        //        {
        //            System.Threading.Thread.Sleep(1000); // Time consuming task in another thread
        //            result = new FileModel("Item " + i);
        //        });

        //        FileModels.Add(result); // Update the UI with the result, in the GUI thread
        //    }

        //}

        public IEnumerable<IResult> LoadDataAsync()
        {
            var loader = new ImageLoaderAsync(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
            yield return loader;
            var result = loader.FileModels;
            LogManager.GetLog(typeof(CaptureViewModel)).Info("取得 {0} 条数据", result.Count);

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) => {
                if (result != null || result.Count > 0)
                {
                    FileModels = new BindableCollection<FileModel>(result);

                    //比较费时
                    NotifyOfPropertyChange(() => FileModels);
                }
            }));
           

        }

        public IEnumerable<IResult> LoadData()
        {
            var loader = new ImageLoader(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
            yield return loader;

            var result = loader.FileModels;

            if (result != null || result.Count > 0)
            {
                FileModels = new BindableCollection<FileModel>(result);
            }

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取图片宽高
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void GetWH(string path, out double width, out double height)
        {
            var bitmap = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(path, UriKind.RelativeOrAbsolute));
            double w = bitmap.Width;
            double h = bitmap.Height;

            double doa = w / h;

            width = doa * 158;
            height = 158;
        }

        /// <summary>
        /// 切换选中状态
        /// </summary>
        /// <param name="image"></param>
        void ToggleImageSource(Image image)
        {
            string path = image.Source.ToString();
            var flag = System.Text.RegularExpressions.Regex.IsMatch(path, @"light");

            //选中状态
            if (flag)
            {
                path = System.Text.RegularExpressions.Regex.Replace(path, "light", "dark");
            }
            else
            {
                path = System.Text.RegularExpressions.Regex.Replace(path, "dark", "light");
            }

            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 去除选中状态
        /// </summary>
        /// <param name="image"></param>
        void UnCheckImageSource(Image image)
        {
            string path = image.Source.ToString();
            var flag = System.Text.RegularExpressions.Regex.IsMatch(path, @"light");

            //选中状态
            if (flag)
            {
                path = System.Text.RegularExpressions.Regex.Replace(path, "light", "dark");
            }

            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            image.Tag = 0;
        }

        /// <summary>
        /// 添加选中状态
        /// </summary>
        /// <param name="image"></param>
        void CheckImageSource(Image image)
        {
            string path = image.Source.ToString();
            var flag = System.Text.RegularExpressions.Regex.IsMatch(path, @"dark");

            //选中状态
            if (flag)
            {
                path = System.Text.RegularExpressions.Regex.Replace(path, "dark", "light");
            }

            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            image.Tag = 1;
        }


        /// <summary>
        /// 保证只有一个是选择状态
        /// </summary>
        void EnsureImageSource(object parent, Image image)
        {
            var sp = parent as StackPanel;
            foreach (var img in sp.Children)
            {
                if (img is Image)
                {
                    if (((Image)img) == image)
                    {
                        CheckImageSource(image);
                        continue;
                    }

                    UnCheckImageSource((Image)img);
                }
            }
        }

        /// <summary>
        /// 切换显示状态
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        void ToggleStackPanel(StackPanel parent, string name)
        {
            var sp = parent as StackPanel;
            if (sp != null)
            {
                foreach (var p in sp.Children)
                {
                    if (p is StackPanel)
                    {
                        if (((StackPanel)p).Name == name)
                        {
                            ((StackPanel)p).Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            ((StackPanel)p).Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        void ShowStackPanel(StackPanel parent, string name)
        {
            var sp = parent as StackPanel;
            if (sp != null)
            {
                foreach (var p in sp.Children)
                {
                    if (p is StackPanel)
                    {
                        if (((StackPanel)p).Name == name)
                        {
                            ((StackPanel)p).Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            ((StackPanel)p).Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        void HideStackPanel(StackPanel parent, string name)
        {
            ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<StackPanel>(parent, name).Visibility = System.Windows.Visibility.Collapsed;
        }


  

        #endregion
        
        #region 工具栏第一行

        public void Edit(object senderParent, object sender, object parent, object child, object cvs)
        {
            var image = sender as Image;
            var sp = senderParent as StackPanel;
            //画布
            var canvas = cvs as InkCanvas;
            if (image != null)
            {
                if (image.Tag.ToString() == "0")
                {
                    if (canvas != null)
                    {
                        canvas.EditingMode = InkCanvasEditingMode.Ink;
                    }
                    EnsureImageSource(sp, image);
                    ShowStackPanel(parent as StackPanel, "EditStackPanel");
                }
                else
                {
                    if (canvas != null)
                    {
                        canvas.EditingMode = InkCanvasEditingMode.None;
                    }
                    UnCheckImageSource(image);
                    HideStackPanel(parent as StackPanel, "EditStackPanel");
                }

              

                

            }

            //ToggleStackPanel(parent as StackPanel, "EditStackPanel");

            //var sp = parent as StackPanel;
            //if (sp != null)
            //{
            //    foreach (var p in sp.Children)
            //    {
            //        if (p is StackPanel)
            //        {
            //            if (((StackPanel)p).Name == "EditStackPanel")
            //            {
            //                ((StackPanel)p).Visibility = System.Windows.Visibility.Visible;
            //            }
            //            else
            //            {
            //                ((StackPanel)p).Visibility = System.Windows.Visibility.Collapsed;
            //            }
            //        }
            //    }
            //}


        }

        //public bool CanAbout
        //{
        //    get
        //    {
        //        if (System.IO.File.Exists(CurrentImage))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        
        public void About(object senderParent, object sender, object parent, object child)
        {
            var image = sender as Image;
            var sp = senderParent as StackPanel;
            if (image != null)
            {
                if (image.Tag.ToString() == "0")
                {
                    EnsureImageSource(sp, image);

                    //ExifModel exif = ShinePhoto.Helpers.ImageHelper.FindExifinfo(CurrentImage);
                    //ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<StackPanel>(parent as StackPanel, "PicasaStackPanel").DataContext = exif;
                    ShowStackPanel(parent as StackPanel, "PicasaStackPanel");
                }
                else
                {
                    UnCheckImageSource(image);
                    HideStackPanel(parent as StackPanel, "PicasaStackPanel");
                }
            }
        }

        public void Voice(object senderParent, object sender, object parent, object child)
        {
            var image = sender as Image;
            var sp = senderParent as StackPanel;
            if (image != null)
            {
                if (image.Tag.ToString() == "0")
                {
                    EnsureImageSource(sp, image);
                    ShowStackPanel(parent as StackPanel, "ShareStackPanel");
                }
                else
                {
                    UnCheckImageSource(image);
                    HideStackPanel(parent as StackPanel, "ShareStackPanel");
                }
            }

            //ToggleStackPanel(parent as StackPanel, "ShareStackPanel");

            //ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<StackPanel>(parent as StackPanel, "ShareStackPanel").Visibility = System.Windows.Visibility.Visible;

        }

        #endregion

        #region 右侧工具栏事件

        public System.Drawing.Color _penColor;

        public System.Drawing.Color PenColor
        {
            get { return _penColor; }
            set
            {
                _penColor = value;
                NotifyOfPropertyChange(() => PenColor);
            }
        }

        /// <summary>
        /// 图片点击事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parent"></param>
        public void ImageTouched(object source, object parent)
        {
            var img = source as Image;
            var sp = parent as UniformGrid;
            if (img != null && img.Tag.ToString() == "0" && sp != null)
            {
                AdjustImageStaus(sp, img);
            }
        }

        /// <summary>
        /// 调整图片状态
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="img"></param>
        void AdjustImageStaus(UniformGrid sp, Image img)
        {
            foreach (var item in sp.Children)
            {
                if (item.GetType() == typeof(Image))
                {
                    var image = item as Image;

                    if (image == img)
                    {
                        var path = img.Source.ToString();
                        var name = path.Substring(0, path.LastIndexOf('.'));
                        var ext = path.Substring(path.LastIndexOf('.'));
                        img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(name + "_Selected" + ext, UriKind.RelativeOrAbsolute));
                        img.Tag = "1";
                        continue;
                    }

                    var fileName = image.Source.ToString().Replace("_Selected", "");
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
                    image.Tag = "0";

                }
            }
        }

        /// <summary>
        /// 清除选中状态
        /// </summary>
        /// <param name="source"></param>
        public void ClearTouched(object source)
        {
            var image = source as Image;
            if (image != null)
            {
                if (image.Tag.ToString() == "0")
                {
                    var path = image.Source.ToString();
                    var name = path.Substring(0, path.LastIndexOf('.'));
                    var ext = path.Substring(path.LastIndexOf('.'));
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(name + "_Selected" + ext, UriKind.RelativeOrAbsolute));
                    image.Stretch = System.Windows.Media.Stretch.UniformToFill;
                    image.Tag = "1";
                }
                else
                {
                    var fileName = image.Source.ToString().Replace("_Selected", "");
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
                    image.Stretch = System.Windows.Media.Stretch.None;
                    image.Tag = "0";
                }
            }
        }

        #endregion

        #region 图片列表导航事件

        public void CurrentImageChanged(object sender, object canvas)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1)
                return;
            CurrentImage = ((FileModel)listBox.SelectedItem).FileName;
            CurrentImageSource = new ImageBrush(new BitmapImage(new Uri(((FileModel)listBox.SelectedItem).FileName, UriKind.RelativeOrAbsolute)));
            ExifModel = ShinePhoto.Helpers.ImageHelper.FindExifinfo(CurrentImage);

            var inkCanvas = canvas as InkCanvas;

            if (inkCanvas != null)
            {
                System.Windows.Rect rect = new System.Windows.Rect(0, 0, inkCanvas.ActualWidth, inkCanvas.ActualHeight);    //設定Rect

                InkCanvas.SetLeft(inkCanvas, rect.Left);    //設定橡皮插圖案Location
                InkCanvas.SetTop(inkCanvas, rect.Top);      //同上
                inkCanvas.Strokes.Erase(rect);    //擦一個矩形居快的筆跡
            }

        }

        public void Prev(object source, object sv)
        {
            var image = source as Image;
            //var scrollViewer = sv as ScrollViewer;
            var surfaceListBox = sv as SurfaceListBox;
            if (image != null && surfaceListBox != null)
            {
                //LogManager.GetLog(typeof(CaptureViewModel)).Info("scrollViewer.Width = {0}, scrollViewer.HorizontalOffset = {1}", scrollViewer.Width, scrollViewer.HorizontalOffset);
                //scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 150.0);
                //if (scrollViewer.HorizontalOffset <= 0)
                //{
                //    PrevVisibility = Visibility.Hidden;
                //}
                //else
                //{
                //    PrevVisibility = Visibility.Visible;
                //}
                //NextVisibility = Visibility.Visible;
                if (_index >= 4)
                {
                    surfaceListBox.ScrollIntoView(surfaceListBox.Items[_index - 4]);
                    _index--;
                    if (_index == 3)
                    {
                        PrevVisibility = Visibility.Hidden;
                    }
                    NextVisibility = Visibility.Visible;
                }
                else
                {
                    PrevVisibility = Visibility.Hidden;
                }
            }

        }

        public void Next(object source, object sv)
        {
            var image = source as Image;
            var surfaceListBox = sv as SurfaceListBox;

            if (image != null && surfaceListBox != null)
            {
                //LogManager.GetLog(typeof(CaptureViewModel)).Info("scrollViewer.Width = {0}, scrollViewer.HorizontalOffset = {1}", scrollViewer.Width, scrollViewer.HorizontalOffset);
                //scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 150.0);
                //if (scrollViewer.HorizontalOffset >= scrollViewer.Width)
                //{
                //    NextVisibility = Visibility.Hidden;
                //}
                //else
                //{
                //    NextVisibility = Visibility.Visible;
                //}
                //PrevVisibility = Visibility.Visible;
                _index = _index + 1;
                if (_index < surfaceListBox.Items.Count)
                {
                    surfaceListBox.ScrollIntoView(surfaceListBox.Items[_index]);
                    PrevVisibility = Visibility.Visible;
                    if (_index == surfaceListBox.Items.Count - 1)
                    {
                        NextVisibility = Visibility.Hidden;
                    }
                }
                else
                {
                    NextVisibility = Visibility.Hidden;
                }
            }
        }

        public void ScrollChanged(object sender)
        {
            LogManager.GetLog(typeof(CaptureViewModel)).Warn("ScrollChanged");
        }

        public void Captured(object sender)
        {
            LogManager.GetLog(typeof(CaptureViewModel)).Warn("Captured");
        }

        public void ListImage_ManipulationStarted(object sender)
        {
            LogManager.GetLog(typeof(CaptureViewModel)).Warn("ListImage_ManipulationStarted");
        }
        
        #endregion

        #region CLR 属性

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

        private bool _isSaving = false;

        /// <summary>
        /// 可视元素个数
        /// </summary>
        private int _index = 3;

        /// <summary>
        /// 画笔轨迹撤销栈
        /// </summary>
        private Stack<Stroke> _undoCollection = new Stack<Stroke>();

        /// <summary>
        /// 图片 EXIF 信息
        /// </summary>
        private ExifModel _exifModel = ShinePhoto.Helpers.ImageHelper.FindExifinfo(AppDomain.CurrentDomain.BaseDirectory + "/Default.jpg");

        public ExifModel ExifModel
        {
            get { return _exifModel; }
            set
            {
                _exifModel = value;
                NotifyOfPropertyChange(() => ExifModel);
            }
        }

        /// <summary>
        /// 当前图片
        /// </summary>
        private string _currentImage = AppDomain.CurrentDomain.BaseDirectory + "/Default.jpg";

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                NotifyOfPropertyChange(() => CurrentImage);
            }
        }

        /// <summary>
        /// 当前图片画刷
        /// </summary>
        private ImageBrush _currentImageSource = new ImageBrush(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Default.jpg", UriKind.RelativeOrAbsolute)));

        public ImageBrush CurrentImageSource
        {
            get { return _currentImageSource; }
            set
            {
                _currentImageSource = value;
                NotifyOfPropertyChange(() => CurrentImageSource);
            }
        }

        private Visibility _nextVisibility = Visibility.Visible;

        public Visibility NextVisibility
        {
            get { return _nextVisibility; }
            set
            {
                _nextVisibility = value;
                NotifyOfPropertyChange(() => NextVisibility);
            }
        }

        private Visibility _prevVisibility = Visibility.Hidden;

        public Visibility PrevVisibility
        {
            get { return _prevVisibility; }
            set
            {
                _prevVisibility = value;
                NotifyOfPropertyChange(() => PrevVisibility);
            }
        }

        #endregion

        #region 依赖属性

        #endregion

        #region 部分组件

        #endregion

        #region EditStackPanel 方法

        public void ChangePen(object source, object canvas)
        {
            var inkCanvas = canvas as InkCanvas;
            if (inkCanvas != null && inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                inkCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Rectangle;
                inkCanvas.DefaultDrawingAttributes.StylusTipTransform = new Matrix(1, 1.5, 2.2, 1, 0, 0);
            }
        } 

        /// <summary>
        /// 重置画板
        /// </summary>
        /// <param name="canvas"></param>
        public void Reset(object canvas)
        {
            var inkCanvas = canvas as InkCanvas;
            if (inkCanvas != null && inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                System.Windows.Rect rect = new System.Windows.Rect(0, 0, inkCanvas.ActualWidth, inkCanvas.ActualHeight);    //設定Rect
                
                InkCanvas.SetLeft(inkCanvas, rect.Left);    //設定橡皮插圖案Location
                InkCanvas.SetTop(inkCanvas, rect.Top);      //同上
                inkCanvas.Strokes.Erase(rect);    //擦一個矩形居快的筆跡
            }
        }

        public bool CanSave
        {
            get
            {
                LogManager.GetLog(typeof(CaptureViewModel)).Info("是否可以进行保存操作 {0} ", !_isSaving);
                return !_isSaving;
            }
        }

        /// <summary>
        /// 保存文件，避免黑边
        /// </summary>
        /// <param name="canvas"></param>
        public void Save(object source, object canvas)
        {
            Storyboard story = new Storyboard();
            _isSaving = true;
            NotifyOfPropertyChange(() => CanSave);
            var image = source as Image;
            var inkCanvas = canvas as InkCanvas;
            if (inkCanvas != null && inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                try
                {
                   
                    #region 通过改变宽高实现动画

                    //DoubleAnimation widthAnimation = new DoubleAnimation();
                    //widthAnimation.From = image.ActualWidth * 0.8;
                    //widthAnimation.To = image.ActualWidth * 1;
                    //widthAnimation.AutoReverse = true;
                    //widthAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    //widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(2.5));


                    //DoubleAnimation heightAnimation = new DoubleAnimation();
                    //heightAnimation.From = image.ActualHeight * 0.8;
                    //heightAnimation.To = image.ActualHeight * 1;
                    //heightAnimation.AutoReverse = true;
                    //heightAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    //heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(2.5));

                    //Storyboard.SetTarget(widthAnimation, image);
                    //Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("Width"));

                    //Storyboard.SetTarget(heightAnimation, image);
                    //Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("Height"));

                    //story.Children.Add(widthAnimation);
                    //story.Children.Add(heightAnimation);

                    #endregion

                    #region ScaleTransform 实现缩放

                    DoubleAnimation scaleXAnimation = new DoubleAnimation();
                    scaleXAnimation.To = 0.95;
                    scaleXAnimation.Duration = new Duration(TimeSpan.FromSeconds(2.5));
                    scaleXAnimation.AutoReverse = true;
                    scaleXAnimation.RepeatBehavior = RepeatBehavior.Forever;


                    Storyboard.SetTarget(scaleXAnimation, image);
                    Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(0).(1)[0].(2)", Image.RenderTransformProperty, TransformGroup.ChildrenProperty, ScaleTransform.ScaleXProperty));
                    story.Children.Add(scaleXAnimation);

                    DoubleAnimation scaleYAnimation = new DoubleAnimation();
                    scaleYAnimation.To = 0.95;
                    scaleYAnimation.AutoReverse = true;
                    scaleYAnimation.Duration = new Duration(TimeSpan.FromSeconds(2.5));
                    scaleYAnimation.RepeatBehavior = RepeatBehavior.Forever;


                    Storyboard.SetTarget(scaleYAnimation, image);
                    Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(0).(1)[0].(2)", Image.RenderTransformProperty, TransformGroup.ChildrenProperty, ScaleTransform.ScaleYProperty));
                    story.Children.Add(scaleYAnimation);

                    #endregion

                    #region 旋转

                    //DoubleAnimation rotateAnimation = new DoubleAnimation();
                    //rotateAnimation.From = 0;
                    //rotateAnimation.To = 180;
                    //rotateAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
                    //rotateAnimation.RepeatBehavior = RepeatBehavior.Forever;


                    //Storyboard.SetTarget(rotateAnimation, image);
                    ////Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("()"));
                    //Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(0).(1)[0].(2)", Image.RenderTransformProperty, TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));
                    //story.Children.Add(rotateAnimation);

                    #endregion

                    story.Begin();

                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Sign\\" + DateTime.Now.ToString("yyyy-MM-dd");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = CurrentImage.Substring(CurrentImage.LastIndexOf('\\') + 1);
                    ShinePhoto.Helpers.ImageHelper.SaveToImage(inkCanvas, dir + "\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss_") + fileName, Helpers.ImageHelper.ImageFormat.JPG);
                    
                    //System.Threading.Thread.Sleep(5000);
                }
                catch(Exception ex)
                {
                    LogManager.GetLog(typeof(CaptureViewModel)).Info("出现异常，异常信息为 {0} ", ex.Message);
                }
                finally
                {
                    _isSaving = false;
                    story.Begin();
                    NotifyOfPropertyChange(() => CanSave);
                }

            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="canvas"></param>
        public void Undo(object canvas)
        {
            var inkCanvas = canvas as InkCanvas;
            if (inkCanvas != null && inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                if (inkCanvas.Strokes.Count > 0)
                {
                    int index = inkCanvas.Strokes.Count - 1;
                    _undoCollection.Push(inkCanvas.Strokes[index]);
                    inkCanvas.Strokes.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="canvas"></param>
        public void Redo(object canvas)
        {
            var inkCanvas = canvas as InkCanvas;
            if (inkCanvas != null && inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                if (_undoCollection.Count > 0)
                {
                    var stroke = _undoCollection.Pop();
                    inkCanvas.Strokes.Add(stroke);
                }
            }
        }

        /// <summary>
        /// 选择颜色
        /// </summary>
        /// <param name="canvas"></param>
        public void Pixel(object source, object parent)
        {
            var image = source as Image;
            var sp = parent as StackPanel;
            if (sp != null && image != null)
            {
                Popup popup = new Popup ();
                ColorCanvas cc = new ColorCanvas();
                //cc.SetValue(ColorCanvas.BorderThicknessProperty, 0);
                cc.SetValue(ColorCanvas.BackgroundProperty, Brushes.Transparent);
                popup.Child = cc;
                popup.PlacementTarget = image;
                popup.Visibility = Visibility.Visible;
            }
        }

        public void SelectedColorChanged(object canvas, object e)
        {
            var inkCanvas = canvas as InkCanvas;
            if (inkCanvas != null && inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                var evt = e as RoutedPropertyChangedEventArgs<Color>;
                if (e != null)
                {
                    inkCanvas.DefaultDrawingAttributes.Color = evt.NewValue;
                }
            }
            
        }

        /// <summary>
        /// 画笔1
        /// </summary>
        /// <param name="source"></param>
        /// <param name="canvas"></param>
        /// <param name="parent"></param>
        public void PenMode1(object source, object canvas, object parent)
        {
            var image = source as Image;
            var inkCanvas = canvas as InkCanvas;
            var sp = parent as StackPanel;
            if (inkCanvas != null && image != null && sp != null)
            {
                EnsureImageSource(sp, image);
                PenModeChanged(inkCanvas, 1.0);
            }
        }

        /// <summary>
        /// 画笔2
        /// </summary>
        /// <param name="source"></param>
        /// <param name="canvas"></param>
        /// <param name="parent"></param>
        public void PenMode2(object source, object canvas, object parent)
        {
            var image = source as Image;
            var inkCanvas = canvas as InkCanvas;
            var sp = parent as StackPanel;
            if (inkCanvas != null && image != null && sp != null)
            {
                EnsureImageSource(sp, image);
                PenModeChanged(inkCanvas, 2.0);
            }
        }

        /// <summary>
        /// 画笔3
        /// </summary>
        /// <param name="source"></param>
        /// <param name="canvas"></param>
        /// <param name="parent"></param>
        public void PenMode3(object source, object canvas, object parent)
        {
            var image = source as Image;
            var inkCanvas = canvas as InkCanvas;
            var sp = parent as StackPanel;
            if (inkCanvas != null && image != null && sp != null)
            {
                EnsureImageSource(sp, image);
                PenModeChanged(inkCanvas, 4.0);
            }
        }

        public void PenModeChanged(InkCanvas inkCanvas, double width)
        {
            LogManager.GetLog(typeof(CaptureViewModel)).Info("画笔宽度 {0} ", width);
            inkCanvas.DefaultDrawingAttributes.Width = inkCanvas.DefaultDrawingAttributes.Height = width;
        }

        #endregion

        #region 监听文件夹事件

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                OnChanged(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                OnDeleted(source, e);
            }

        }
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("文件新建事件处理逻辑 {0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name));
            FileModels.Insert(0, new FileModel { FileName = e.FullPath, Height = 158, Width = 282 });
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("文件改变事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name));
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("文件删除事件处理逻辑{0}  {1}   {2}", e.ChangeType, e.FullPath, e.Name));
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("文件重命名事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name));
        }

        #endregion

        #region InkCanvas 事件

        private Dictionary<Stroke, System.Threading.Timer> dicTimer = new Dictionary<Stroke, System.Threading.Timer>();

        public void StrokeCollected(object args)
        {
            var e = args as InkCanvasStrokeCollectedEventArgs;
           
            System.Threading.Timer t1 = new System.Threading.Timer(new System.Threading.TimerCallback(ChangeOpacity), e.Stroke, 100, 100);
            dicTimer.Add(e.Stroke, t1);

            //if (e.Stroke.StylusPoints.Count > 1)
            //{
            //    UpdateLine(e.Stroke);
            //}

        }

        private void ChangeOpacity(Object obj)
        {
            Stroke line = obj as Stroke;
            Color linecolor = line.DrawingAttributes.Color;
            if (linecolor.ScA > 0)
                linecolor.ScA -= 0.1f;
            (GetView() as ShinePhoto.Views.CaptureView).Dispatcher.BeginInvoke(new System.Action(() => { line.DrawingAttributes.Color = linecolor; }));
            if (linecolor.A <= 0)
            {
                dicTimer[line].Dispose();
                dicTimer.Remove(line);
            }
        }

        /// <summary>
        /// 在InkCanvas上画直虚线
        /// 还是跟上面一样取起始点和终点，不同点是：在两点间绘制许多点，然后将相邻的两点连接成一个笔画。这样一个直虚线变好了。
        /// </summary>
        /// <param name="currentStroke"></param>
        private void UpdateLine(Stroke currentStroke)
        {
            var inkC = GetView() as ShinePhoto.Views.CaptureView;
            StylusPoint beginPoint = currentStroke.StylusPoints[0];//起始点
            StylusPoint endPoint = currentStroke.StylusPoints.Last();//终点
            inkC.SignCanvas.Strokes.Remove(currentStroke);//移除原来笔画
            int dotTime = 0;
            int intervalLen = 6;//步长
            double lineLen = Math.Sqrt(Math.Pow(beginPoint.X - endPoint.X, 2) + Math.Pow(beginPoint.Y - endPoint.Y, 2));//线的长度
            Point currentPoint = new Point(beginPoint.X, beginPoint.Y);
            double relativaRate = Math.Abs(endPoint.Y - beginPoint.Y) * 1.0 / Math.Abs(endPoint.X - beginPoint.X);
            double angle = Math.Atan(relativaRate) * 180 / Math.PI;//直线的角度大小，无需考虑正负
            int xOrientation = endPoint.X > beginPoint.X ? 1 : -1;//判断新生成点的X轴方向
            int yOrientation = endPoint.Y > beginPoint.Y ? 1 : -1;
            if (lineLen < intervalLen)
            {
                return;
            }
            while (dotTime * intervalLen < lineLen)
            {
                double x = currentPoint.X + dotTime * intervalLen * Math.Cos(angle * Math.PI / 180) * xOrientation;
                double y = currentPoint.Y + dotTime * intervalLen * Math.Sin(angle * Math.PI / 180) * yOrientation;
                List<Point> pL = new List<Point>();
                pL.Add(new Point(x, y));
                x += intervalLen * Math.Cos(angle * Math.PI / 180) * xOrientation;
                y += intervalLen * Math.Sin(angle * Math.PI / 180) * yOrientation;
                pL.Add(new Point(x, y));
                StylusPointCollection spc = new StylusPointCollection(pL);//相邻两点作为一个笔画
                Stroke stroke = new Stroke(spc);
                stroke.DrawingAttributes = inkC.SignCanvas.DefaultDrawingAttributes.Clone();
                inkC.SignCanvas.Strokes.Add(stroke);
                dotTime += 2;
            }
        }

        /// <summary>
        /// 在InkCanvas上画直线
        /// 在StrokeCollected事件中进行修正，StrokeCollected事件在单个笔画结束后触发。可以取出笔画的起始点(BeginPoint)和终点(EndPoint),然后使用该两点新建笔画即可。
        /// </summary>
        /// <param name="currentStroke"></param>
        //private void UpdateLine(Stroke currentStroke)
        //{
        //    var inkC = GetView() as ShinePhoto.Views.CaptureView;

        //    StylusPoint beginPoint = currentStroke.StylusPoints[0];//起始点
        //    StylusPoint endPoint = currentStroke.StylusPoints.Last();//终点
        //    inkC.SignCanvas .Strokes.Remove(currentStroke);//移除原来的笔画
        //    List<Point> pointList = new List<Point>();
        //    pointList.Add(new Point(beginPoint.X, beginPoint.Y));
        //    pointList.Add(new Point(endPoint.X, endPoint.Y));
        //    StylusPointCollection point = new StylusPointCollection(pointList);
        //    Stroke stroke = new Stroke(point);//用两点实现笔画
        //    stroke.DrawingAttributes = inkC.SignCanvas.DefaultDrawingAttributes.Clone();
        //    inkC.SignCanvas.Strokes.Add(stroke);
        //}

        private int intervalLen = 5;

        /// <summary>
        /// 在InkCanvas上画弯虚线
        /// 在InkCanvas_StrokeCollected方法的参数属性中可以获得线上的点的坐标。
        /// 2）生成StylusPointCollection collection=currentStroke.StylusPoints，用来存放当前笔画的所有点
        /// 2）生成一个List<Point> allPointList,用来存放最终生成的点
        /// 2) 将起始点=》allPointList，起始点-》currentPoint
        /// 3）遍历collection，IF currentPoint与找到点(item)的距离==步长 THEN item=>allPointList;item=>currentPoint,取下一个点
        /// IF currentPoint与找到点(item)的距离大于步长 THEN 在currentPoint与item线上找到一个点，使得与currentPoint的距离=步长，item=>allPointList;item=>currentPoint，继续当前点
        /// IF currentPoint与找到点(item)的距离小于步长 取下一个点
        /// </summary>
        /// <param name="currentStroke"></param>
        //private void UpdateLine(Stroke currentStroke)
        //{
        //    var inkC = GetView() as ShinePhoto.Views.CaptureView;

        //    inkC.SignCanvas.Strokes.Remove(currentStroke);
        //    StylusPointCollection collection = currentStroke.StylusPoints;
        //    List<Point> allSelectedPoint = new List<Point>();
        //    Point currentPoint = new Point(collection[0].X, collection[0].Y);
        //    allSelectedPoint.Add(currentPoint);
        //    for (int i = 0; i < collection.Count; i++)
        //    {
        //        var item = collection[i];
        //        double length = Math.Sqrt(Math.Pow(item.X - currentPoint.X, 2) + Math.Pow(item.Y - currentPoint.Y, 2));
        //        if ((int)(length + 0.5) == (int)intervalLen || length == intervalLen)
        //        {

        //            currentPoint = new Point(item.X, item.Y);
        //            allSelectedPoint.Add(currentPoint);
        //        }
        //        else if (length > intervalLen)
        //        {
        //            double relativaRate = Math.Abs(item.Y - currentPoint.Y) * 1.0 / Math.Abs(item.X - currentPoint.X);
        //            double angle = Math.Atan(relativaRate) * 180 / Math.PI;
        //            int xOrientation = item.X > currentPoint.X ? 1 : -1;
        //            int yOrientation = item.Y > currentPoint.Y ? 1 : -1;
        //            double x = currentPoint.X + intervalLen * Math.Cos(angle * Math.PI / 180) * xOrientation;
        //            double y = currentPoint.Y + intervalLen * Math.Sin(angle * Math.PI / 180) * yOrientation;
        //            currentPoint = new Point(x, y);
        //            allSelectedPoint.Add(currentPoint);
        //            i--;//很重要，继续当前点
        //        }
        //    }
        //    for (int j = 0; j < allSelectedPoint.Count; j++)
        //    {
        //        List<Point> p = new List<Point>();
        //        p.Add(allSelectedPoint[j]);
        //        if (j < allSelectedPoint.Count - 1)
        //        {
        //            j++;
        //        }
        //        p.Add(allSelectedPoint[j]);
        //        StylusPointCollection spc = new StylusPointCollection(p);
        //        Stroke stroke = new Stroke(spc);
        //        inkC.SignCanvas.Strokes.Add(stroke);
        //    }
        //}

        #endregion

    }
}
