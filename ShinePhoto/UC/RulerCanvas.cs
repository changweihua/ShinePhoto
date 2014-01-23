using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ShinePhoto.UC
{
    public class RulerCanvas : Canvas
    {
        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc);
            //Pen pen = new Pen(Brushes.LightGray, 1);

            //for (int x = 100; x < this.ActualWidth; x += 100)
            //{
            //    dc.DrawLine(pen, new Point(x, 0), new Point(x, this.ActualHeight));
            //}

            //for (int y = 100; y < this.ActualHeight; y += 100)
            //{
            //    dc.DrawLine(pen, new Point(0, y), new Point(this.ActualWidth, y));
            //}

        }

    }
}
