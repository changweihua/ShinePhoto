using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using ShinePhoto.Models;

namespace ShinePhoto.Loaders
{
    /// <summary>
    /// 异步
    /// </summary>
    public class ImageLoaderAsync : IResult
    {
        public List<FileModel> FileModels { private set; get; }
        private readonly System.Action _action;
        private readonly string _str;

        public ImageLoaderAsync(System.Action action)
        {
            this._action = action;
            FileModels = new List<FileModel>();
        }

        public ImageLoaderAsync(string str)
        {
            this._str = str;
            FileModels = new List<FileModel>();
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = (sender, args) =>
        {
            LogManager.GetLog(typeof(ImageLoaderAsync)).Info(((ImageLoaderAsync)sender)._str);
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

        public void Execute(ActionExecutionContext context)
        {
            using (var backgroundWorker = new System.ComponentModel.BackgroundWorker())
            {
                backgroundWorker.DoWork += (e, sender) =>
                {
                    double width, height;

                    var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");

                    for (int i = 0; i < arr.Length; i++)
                    {
                        GetWH(arr[i], out width, out height);
                        FileModels.Add(new FileModel { FileName = arr[i], Height = height, Width = width });
                    }
                };
                backgroundWorker.RunWorkerCompleted += (e, sender) =>
                {
                    LogManager.GetLog(typeof(ImageLoaderAsync)).Info("异步加载完毕");
                    Completed(this, new ResultCompletionEventArgs());
                };
                backgroundWorker.RunWorkerAsync();
            }

        }
    }

    /// <summary>
    /// 同步
    /// </summary>
    public class ImageLoader : IResult
    {
        public List<FileModel> FileModels { private set; get; }

        readonly string _str;
        public ImageLoader(string str)
        {
            _str = str;
            FileModels = new List<FileModel>();
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = (sender, args) =>
        {
            LogManager.GetLog(typeof(ImageLoader)).Info(((ImageLoader)sender)._str);
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

        public void Execute(ActionExecutionContext context)
        {
            LogManager.GetLog(typeof(ImageLoader)).Info(_str + context.View);

            double width, height;

            var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");

            for (int i = 0; i < arr.Length; i++)
            {
                GetWH(arr[i], out width, out height);
                FileModels.Add(new FileModel { FileName = arr[i], Height = height, Width = width });
            }

            Completed(this, new ResultCompletionEventArgs());//这个方法一定要加到这里，这个方法完成后才会执行后边的方法
        }
    }
}
