using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Media.Animation;
using Caliburn.Micro;
using System.Windows.Threading;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(MessageBoxViewModel))]
    public class MessageBoxViewModel : Screen
    {
        DispatcherTimer dt = new DispatcherTimer();

        public void LoadUserControl(object source)
        {
            var view = source as ShinePhoto.Views.MessageBoxView;

            if (view != null)
            {
                try
                {
                    var story = (Storyboard)view.Resources["show"];
                    story.Completed += (a, b) =>
                    {
                        dt.Interval = TimeSpan.FromSeconds(1);
                        dt.Tick += new EventHandler(dt_Tick);
                        dt.Start();
                    };
                    story.Begin();
                }
                catch (Exception ex)
                {
                    LogManager.GetLog(typeof(SplashViewModel)).Warn(ex.Message);
                }

            }

        }

        private void dt_Tick(object sender, EventArgs e)
        {
            var view = GetView() as ShinePhoto.Views.MessageBoxView;

            if (view != null)
            {
                try
                {
                    var story = (Storyboard)view.Resources["hide"];
                    story.Completed += (a, b) =>
                    {
                        dt.Stop();
                    };
                }
                catch (Exception ex)
                {
                    LogManager.GetLog(typeof(SplashViewModel)).Warn(ex.Message);
                }

            }
        }

    }
}
