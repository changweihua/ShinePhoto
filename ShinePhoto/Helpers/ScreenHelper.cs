using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ShinePhoto.Helpers
{
    /// <summary>
    /// 屏幕帮助类
    /// </summary>
    public class ScreenHelper
    {
        public static Area GetWorkArea()
        {
            double x = SystemParameters.WorkArea.Width;//得到屏幕工作区域宽度
            double y = SystemParameters.WorkArea.Height;//得到屏幕工作区域高度

            return new Area { Width = x, Height = y };
        }

        public static Area GetScreenArea()
        {
            double x = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            double y = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度

            return new Area { Width = x, Height = y };
        }
    }

    public struct Area
    {
        public double Width;
        public double Height;

        public string ToXString()
        {
            return string.Format("宽度 {0}, 高度 {1}", Width, Height);
        }
    }
}
