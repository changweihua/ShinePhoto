using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShinePhoto.Extensions
{
    public class FlatWindowManager : WindowManager
    {
        protected override System.Windows.Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = base.EnsureWindow(model, view, isDialog);

            window.WindowStyle = WindowStyle.None;
            //window.Icon = new BitmapImage(new Uri("Images/Photo.ico", UriKind.RelativeOrAbsolute));
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.ResizeMode = ResizeMode.NoResize;

            return window;
        }



    }
}
