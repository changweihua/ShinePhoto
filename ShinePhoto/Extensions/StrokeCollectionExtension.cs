using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows;
using System.Windows.Input;

namespace ShinePhoto.Extensions
{
    /// <summary>
    /// StrokeCollection类扩展方法——边界框计算
    /// </summary>
    public static class StrokeCollectionExtension
    {
        /// <summary>
        /// 计算笔画集合的边界框
        /// </summary>
        /// <param name="sc">笔画集合</param>
        /// <returns>矩形区域</returns>
        public static Rect Bounding(this StrokeCollection sc)
        {
            if (sc == null || sc.Count == 0)
                throw new InvalidOperationException();

            Double xMin = Double.MaxValue;  // X坐标最小值
            Double xMax = Double.MinValue;  // X坐标最大值
            Double yMin = Double.MaxValue;  // Y坐标最小值
            Double yMax = Double.MinValue;  // Y坐标最大值
            foreach (Stroke s in sc)
            {
                if (s.StylusPoints == null || s.StylusPoints.Count == 0)
                    continue;

                foreach (StylusPoint sp in s.StylusPoints)
                {
                    if (sp.X < xMin)
                        xMin = sp.X;
                    else if (sp.X > xMax)
                        xMax = sp.X;

                    if (sp.Y < yMin)
                        yMin = sp.Y;
                    else if (sp.Y > yMax)
                        yMax = sp.Y;
                }
            }

            if (xMin > xMax || yMin > yMax)
                throw new InvalidOperationException();

            return new Rect { X = xMin, Y = yMin, Width = xMax - xMin, Height = yMax - yMin };
        }
    }
}
