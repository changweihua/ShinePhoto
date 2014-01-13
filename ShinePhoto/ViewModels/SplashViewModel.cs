using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(SplashViewModel))]
    public class SplashViewModel : Screen
    {
        Storyboard stdStart, stdEnd; 

        public void LoadUserControl(object source)
        {
            var view = source as ShinePhoto.Views.SplashView;

            TextBlock tbName = null;
            if (view != null)
            {
                try
                {
                    tbName = ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<TextBlock>(view, "tbName");
                    SwipeWords(tbName, view);
                    stdStart = (Storyboard)view.Resources["start"];
                    stdEnd = (Storyboard)view.Resources["end"];
                    stdStart.Completed += (a, b) =>
                    {
                        view.root.Clip = null;
                        Func<bool> func = InitApp;

                        //在UI线程中得到异步任务的返回值，并更新UI
                        //必须在UI线程中执行 
                        Action<IAsyncResult> resultHandler = delegate(IAsyncResult asyncResult)
                        {
                            bool flag = func.EndInvoke(asyncResult);
                            SystemInfo = "123456";
                            var win = GetView() as Window;
                            Application.Current.MainWindow = null;
                            win.DialogResult = true;
                            win.Close();
                        };

                        var result = func.BeginInvoke((asyncResult) =>
                        {
                            (GetView() as Window).Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, resultHandler, asyncResult);
                        }, null);
                    };
                  
                    stdStart.Begin();
                }
                catch (Exception ex)
                {
                    LogManager.GetLog(typeof(SplashViewModel)).Warn(ex.Message);
                }
                
            }

        }

        #region 属性

        private string _systemInfo = "开始加载";

        /// <summary>
        /// 加载进度
        /// </summary>
        public string SystemInfo
        {
            get { return _systemInfo; }
            set { _systemInfo = value; NotifyOfPropertyChange(() => SystemInfo); }
        }

        private string _screenDimension = "开始加载";

        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        public string ScreenDimension
        {
            get { return _screenDimension; }
            set { _screenDimension = value; NotifyOfPropertyChange(() => ScreenDimension); }
        }

        #endregion

        public void MoveUserControl()
        {
            var win = GetView() as Window;
            if (win != null)
            {
                win.DragMove();
            }
        }

        private bool InitApp()
        {
            bool result = true;

            try
            {
                ScreenDimension = string.Format("屏幕分辨率 {0}*{1}", ShinePhoto.Helpers.ScreenHelper.GetScreenArea().Width, ShinePhoto.Helpers.ScreenHelper.GetScreenArea().Height);
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                //string.Format("出现异常，异常信息为 {0}", ex.Message)
                LogManager.GetLog(typeof(SplashViewModel)).Error(ex);
            }

            return result;
        }

        private void SwipeWords(TextBlock tb, FrameworkElement container)
        {
            Storyboard perChar = new Storyboard();
            tb.TextEffects = new TextEffectCollection();
            for (int i = 0; i < tb.Text.Length; i++)
            {
                TextEffect effect = new TextEffect();
                effect.Transform = new TranslateTransform();
                effect.PositionStart = i;
                effect.PositionCount = 1;
                tb.TextEffects.Add(effect);
                DoubleAnimation anim = new DoubleAnimation();
                anim.To = 5;
                anim.AccelerationRatio = 0.3;
                anim.DecelerationRatio = 0.3;
                anim.AutoReverse = true;
                anim.Duration = TimeSpan.FromSeconds(1);
                anim.BeginTime = TimeSpan.FromMilliseconds(250 * i);
                anim.RepeatBehavior = RepeatBehavior.Forever;
                Storyboard.SetTargetProperty(anim, new PropertyPath("TextEffects[" + i + "].Transform.Y"));
                Storyboard.SetTargetName(anim, tb.Name);
                perChar.Children.Add(anim);
            }
            perChar.Begin(container);
        }
    }
}
