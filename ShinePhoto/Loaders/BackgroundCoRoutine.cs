using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel;

namespace ShinePhoto.Loaders
{
    public class BackgroundCoRoutine : IResult
    {
        private readonly System.Action _action;

        public BackgroundCoRoutine(System.Action action)
        {
            this._action = action;
        }

        public void Execute(ActionExecutionContext context)
        {
            using (var backgroundWorker = new BackgroundWorker())
            {
                backgroundWorker.DoWork += (e, sender) => _action();
                backgroundWorker.RunWorkerCompleted += (e, sender) => Completed(this, new ResultCompletionEventArgs());
                backgroundWorker.RunWorkerAsync();
            }
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = (sender, args) =>
        {
            LogManager.GetLog(typeof(BackgroundCoRoutine)).Info(((BackgroundCoRoutine)sender)._action.Method.Name);
        };

        void GetWH(string path, out double width, out double height)
        {
            var bitmap = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(path, UriKind.RelativeOrAbsolute));
            double w = bitmap.Width;
            double h = bitmap.Height;

            double doa = w / h;

            width = doa * 158;
            height = 158;
        }
    }
}
