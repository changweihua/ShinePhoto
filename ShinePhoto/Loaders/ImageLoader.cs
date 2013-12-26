using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace ShinePhoto.Loaders
{
    public class ImageLoader : IResult
    {
        readonly string _str;
        public ImageLoader(string str)
        {
            _str = str;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = (sender, args) =>
        {
            //MessageBox.Show(((Loader)sender)._str);
        };

        public void Execute(ActionExecutionContext context)
        {
            //Debug.Show(_str + context.View);
            Completed(this, new ResultCompletionEventArgs());//这个方法一定要加到这里，这个方法完成后才会执行后边的方法
        }
    }
}
