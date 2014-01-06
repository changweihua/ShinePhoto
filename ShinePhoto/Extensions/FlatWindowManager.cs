using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ShinePhoto.Extensions
{
    /// <summary>
    /// 自定义窗体管理，实现扁平化效果
    /// </summary>
    public class FlatWindowManager : WindowManager
    {
        protected override System.Windows.Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = base.EnsureWindow(model, view, isDialog);

            window.WindowStyle = WindowStyle.None;
            //window.Icon = new BitmapImage(new Uri("Images/Photo.ico", UriKind.RelativeOrAbsolute));
            //window.SizeToContent = SizeToContent.WidthAndHeight;
            window.SizeToContent = SizeToContent.Manual;
            window.ResizeMode = ResizeMode.NoResize;
            window.WindowState = WindowState.Maximized;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window.Background = new SolidColorBrush(Colors.Transparent);

            return window;
        }

    }
}
