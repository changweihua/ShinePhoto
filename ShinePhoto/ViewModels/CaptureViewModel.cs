using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Models;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 即拍即得视图 ViewModel
    /// </summary>
    [Export(typeof(CaptureViewModel))]
    public class CaptureViewModel : PropertyChangedBase, IShellView
    {
        /// <summary>
        /// 图片集合
        /// </summary>
        public BindableCollection<FileModel> FileModels { get; private set; }

        /// <summary>
        /// 事件聚合器
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public CaptureViewModel(IEventAggregator eventAggregator)
        {
             _eventAggregator = eventAggregator;
            FileModels = new BindableCollection<FileModel>();
            
            double width, height;

            var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
            
            for (int i = 0; i < arr.Length; i++)
            {
                GetWH(arr[i], out width, out height);
                FileModels.Add(new FileModel { FileName = arr[i], Height = height, Width = width });
            }
        }

        /// <summary>
        /// 获取图片宽高
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void GetWH(string path, out double width, out double height)
        {
            var bitmap = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(path, UriKind.RelativeOrAbsolute));
            double w = bitmap.Width;
            double h = bitmap.Height;

            double doa = w / h;

            width = doa * 158;
            height = 158;
        }

        private string _currentImageSource;

        public string CurrentImageSource
        {
            get { return _currentImageSource; }
            set {
                _currentImageSource = value;
                NotifyOfPropertyChange(() => CurrentImageSource);
            }
        }

        public System.Drawing.Color _penColor;

        public System.Drawing.Color PenColor
        {
            get { return _penColor; }
            set {
                _penColor = value;
                NotifyOfPropertyChange(() => PenColor);
            }
        }

        public void ImageTouched(object source, object parent)
        {
            var img = source as Image;
            var sp = parent as UniformGrid;
            if (img != null && img.Tag.ToString() == "0" && sp != null)
            {
                AdjustImageStaus(sp, img);
            }
        }

        void AdjustImageStaus(UniformGrid sp, Image img)
        {
            foreach (var item in sp.Children)
            {
                if (item.GetType() == typeof(Image))
                {
                    var image = item as Image;

                    if (image == img)
                    {
                        var path = img.Source.ToString();
                        var name = path.Substring(0, path.LastIndexOf('.'));
                        var ext = path.Substring(path.LastIndexOf('.'));
                        img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(name + "_Selected" + ext, UriKind.RelativeOrAbsolute));
                        img.Tag = "1";
                        continue;
                    }

                    var fileName = image.Source.ToString().Replace("_Selected", "");
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
                    image.Tag = "0";

                }
            }
        }

        public void ClearTouched(object source)
        {
            var image = source as Image;
            if (image != null)
            {
                if (image.Tag.ToString() == "0")
                {
                    var path = image.Source.ToString();
                    var name = path.Substring(0, path.LastIndexOf('.'));
                    var ext = path.Substring(path.LastIndexOf('.'));
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(name + "_Selected" + ext, UriKind.RelativeOrAbsolute));
                    image.Stretch = System.Windows.Media.Stretch.UniformToFill;
                    image.Tag = "1";
                }
                else
                {
                    var fileName = image.Source.ToString().Replace("_Selected", "");
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
                    image.Stretch = System.Windows.Media.Stretch.None;
                    image.Tag = "0";
                }
            }
        }

    }
}
