using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Interface;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 炫影展示视图 ViewModel
    /// </summary>
    [Export(typeof(ShineDisplayViewModel))]
    public class ShineDisplayViewModel : Screen,IShellView
    {
        public ShineDisplayViewModel()
        {
            if (Execute.InDesignMode)
                LoadDesignData();
        }

        private void LoadDesignData()
        {
            
        }

        public void ShowPopup()
        {
            var manager = IoC.Get<IWindowManager>();
            manager.ShowPopup(new MessageViewModel());
        }

        public void ShowMessageBox(object obj)
        {
            //object win = parent as Window;
            var container = obj as StackPanel;
            var view = GetView() as ShinePhoto.Views.ShineDisplayView;
            if (container != null && view != null)
            {
                container.Children.Add(new ShinePhoto.Views.MessageBoxView());
            }
        }

        public void ShowImage(object source)
        {
            var view = GetView() as ShinePhoto.Views.ShineDisplayView;
            var image = source as Image;
            if (view != null && image != null)
            {
                LogManager.GetLog(typeof(ShineDisplayViewModel)).Info("开始显示图片");
                ShinePhoto.UC.ImageViewUserControl ivus = new UC.ImageViewUserControl();
                ivus.canvas.Children.Add(CreateImage(image.Tag.ToString()));
                ivus.SetValue(Canvas.RightProperty, -10.0);
                ivus.SetValue(Canvas.BottomProperty, -15.0);
                view.ImageViewCanvas.Children.Add(ivus);

                ivus.CloseButton.Click += (sender, e) =>
                {
                    view.ImageViewCanvas.Children.Remove(ivus);
                };
            }
        }

        #region 图片展示

        UIElement last;

        public void ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            LogManager.GetLog(typeof(ShineDisplayViewModel)).Info("ManipulationInertiaStarting");
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
            LogManager.GetLog(typeof(ShineDisplayViewModel)).Info("ManipulationStarting");
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
            LogManager.GetLog(typeof(ShineDisplayViewModel)).Info("ManipulationDelta");
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

        #region 导航按钮

        public void NextPage()
        {
             var view = GetView() as ShinePhoto.Views.ShineDisplayView;

             if (view != null)
             {
                 _pageIndex += 1;
                 var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
                 view.QQ.ItemsSource = Pagination<string>(arr);
             }
        }

        public void PrevPage()
        {
            var view = GetView() as ShinePhoto.Views.ShineDisplayView;

            if (view != null)
            {
                _pageIndex -= 1;
                var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
                view.QQ.ItemsSource = Pagination<string>(arr);
            }
        }

        #endregion

        #region UserControl 方法

        public void LoadUserControl(object source)
        {
            var view = source as ShinePhoto.Views.ShineDisplayView;

            if (view != null)
            {
                var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
                view.QQ.ItemsSource = Pagination<string>(arr);
                //view.itemsControl.ItemsSource = typeof(System.Windows.Media.Colors).GetProperties();
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
            image.Width = 1024;
            image.Height = 768;
            image.IsManipulationEnabled = true;
            image.RenderTransform = new MatrixTransform();
            image.SetValue(Canvas.LeftProperty, (1920 - 1024) / 2.0);
            image.SetValue(Canvas.TopProperty, (1080 - 768) / 2.0);
            image.Style = Application.Current.FindResource("FlyingImage") as Style;

            return image;
        }

        private IEnumerable<T> Pagination<T>(IEnumerable<T> list)
        {
            return list.Skip(_pageIndex * _pageCount).Take(_pageCount);
        }

        #endregion

        #region 底部工具栏

        public void ShutDown()
        {
            Application.Current.Shutdown();
        }

        public void SystemCog()
        {
            var cogScreen = IoC.Get<SystemCogViewModel>();
            var manager = new ShinePhoto.Extensions.MessageWindowManager();

            var settings = new Dictionary<string, object>
                {
                    { "AllowsTransparency", true},
                    { "Background", System.Windows.Media.Brushes.Transparent},
                    { "WindowStyle", System.Windows.WindowStyle.None},
                    { "WindowStartupLocation", System.Windows.WindowStartupLocation.CenterScreen }
                };

            bool? flag = manager.ShowDialog(cogScreen, null, settings);

            if (flag.Value)
            {
                LogManager.GetLog(typeof(AppBootstrapper)).Info("程序初始化成功");
            }
            else
            {
                LogManager.GetLog(typeof(AppBootstrapper)).Info("程序初始化失败");
            }
        }

        #endregion

        #region 变量

        private int _pageIndex = 0;
        private int _pageCount = 5;

        #endregion

        #region 属性

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
