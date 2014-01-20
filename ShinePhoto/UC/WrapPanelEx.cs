using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ShinePhoto.UC
{
    public class MyPanel : Panel
    {
        //默认动画时间
        public static readonly Duration DefDuration = new Duration(TimeSpan.FromMilliseconds(300));

        protected override Size MeasureOverride(Size availableSize)
        {
            var retSize = new Size();
            foreach (UIElement ui in InternalChildren)
            {
                ui.Measure(new Size(availableSize.Width, availableSize.Height));
                retSize.Height += ui.DesiredSize.Height;
                retSize.Width = Math.Max(retSize.Width, ui.DesiredSize.Width);
            }
            return retSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var next = new Point();
            foreach (UIElement ui in InternalChildren)
            {
                ui.Arrange(new Rect(new Point(), ui.DesiredSize));

                var transform = ui.RenderTransform as TranslateTransform;
                if (transform == null)
                    ui.RenderTransform = transform = new TranslateTransform();

                transform.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(next.Y, DefDuration));

                next.Y += ui.RenderSize.Height;
            }
            return finalSize;
        }
    }

    public class WrapPanelEx : WrapPanel
    { 

         public static readonly DependencyProperty ColumnsProperty;

         static WrapPanelEx()  
        {
            ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(WrapPanelEx),  
                    new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure));  
  
        }  
  
        public int Columns  
        {  
            set { SetValue(ColumnsProperty, value); }  
            get { return (int)GetValue(ColumnsProperty); }  
        }  
  
        public int Rows  
        {  
            get { return (InternalChildren.Count + Columns - 1) / Columns; }  
        }  
  
        protected override Size MeasureOverride(Size sizeAvailable)  
        {  
            // 根据统一的行与列,计算孩子的尺寸  
            Size sizeChild = new Size(sizeAvailable.Width / Columns, sizeAvailable.Height / Rows);  
  
            // 用来累积最大宽度和高度的变量  
            double maxwidth = 0;  
            double maxheight = 0;  
  
            foreach (UIElement child in InternalChildren)  
            {  
  
                // 调用每个孩子的Measure  
                child.Measure(sizeChild);  
  
                // 为孩子设置适合的尺寸  
                maxwidth = Math.Max(maxwidth, child.DesiredSize.Width);  
                maxheight = Math.Max(maxheight, child.DesiredSize.Height);  
            }  
            // 为grid本身计算尺寸  
            return new Size(Columns * maxwidth, Rows * maxheight);  
        }  
  
        protected override Size ArrangeOverride(Size sizeFinal)  
        {  
            Size sizeChild = new Size(sizeFinal.Width / Columns,  
                                      sizeFinal.Height / Rows);  
  
            for (int index = 0; index < InternalChildren.Count; index++)  
            {  
                int row = index / Columns;  
                int col = index % Columns;  
  
                // 计算每个孩子在sizeFinal内的矩形  
                Rect rectChild = new Rect(new Point(col * sizeChild.Width, row * sizeChild.Height), sizeChild);  
  
                // 然后调用Arrange更新布局  
                InternalChildren[index].Arrange(rectChild);  
            }  
            return sizeFinal;  
        }  

        ////默认动画时间
        //public static readonly Duration DefDuration = new Duration(TimeSpan.FromMilliseconds(300));

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    //var retSize = new Size();
        //    //foreach (UIElement ui in InternalChildren)
        //    //{
        //    //    ui.Measure(new Size(availableSize.Width, availableSize.Height));
        //    //    retSize.Width += ui.DesiredSize.Width;
        //    //    retSize.Height = Math.Max(retSize.Height, ui.DesiredSize.Height);
        //    //}
        //    //return retSize;

        //    double totalWidth = 0;
        //    double totalHeight = 0;

        //    foreach (UIElement child in Children)
        //    {
        //        child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        //        Size childSize = child.DesiredSize;
        //        totalWidth += childSize.Width;
        //        totalHeight += childSize.Height;
        //    }

        //    return new Size(totalWidth, totalHeight);
        //}

        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    //var next = new Point();
        //    //foreach (UIElement ui in InternalChildren)
        //    //{
        //    //    ui.Arrange(new Rect(new Point(), ui.DesiredSize));

        //    //    var transform = ui.RenderTransform as TranslateTransform;
        //    //    if (transform == null)
        //    //        ui.RenderTransform = transform = new TranslateTransform();

        //    //    transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(next.X, DefDuration));

        //    //    next.X += ui.RenderSize.Width;
        //    //}
        //    //return finalSize;

        //    Point currentPosition = new Point();

        //    foreach (UIElement child in Children)
        //    {
        //        Rect childRect = new Rect(currentPosition, child.DesiredSize);
        //        child.Arrange(childRect);
        //        currentPosition.Offset(childRect.Width, childRect.Height);
        //    }

        //    return new Size(currentPosition.X, currentPosition.Y);
        //}
    }
}
