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
using ShinePhoto.Loaders;
using Microsoft.Surface.Presentation.Controls;

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
            //Caliburn.Micro.Coroutine.BeginExecute((new[] { new ImageLoader("") }).ToList<IResult>().GetEnumerator());
            
             _eventAggregator = eventAggregator;

             //Caliburn.Micro.Coroutine.BeginExecute(LoadData().GetEnumerator());
             Caliburn.Micro.Coroutine.BeginExecute(LoadDataAsync().GetEnumerator());
        }

        #region 异步事件

        public IEnumerable<IResult> LoadDataAsync()
        {
            var loader = new ImageLoaderAsync(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
            yield return loader;
            var result = loader.FileModels;
            LogManager.GetLog(typeof(CaptureViewModel)).Info("取得 {0} 条数据", result.Count);

            if (result != null || result.Count > 0)
            {
                FileModels = new BindableCollection<FileModel>(result);

                //比较费时
                NotifyOfPropertyChange(() => FileModels);
            }

        }

        public IEnumerable<IResult> LoadData()
        {
            var loader = new ImageLoader(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
            yield return loader;

            var result = loader.FileModels;

            if (result != null || result.Count > 0)
            {
                FileModels = new BindableCollection<FileModel>(result);
            }

        }

        #endregion

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
 

        #region 右侧工具栏事件

        public System.Drawing.Color _penColor;

        public System.Drawing.Color PenColor
        {
            get { return _penColor; }
            set
            {
                _penColor = value;
                NotifyOfPropertyChange(() => PenColor);
            }
        }

        /// <summary>
        /// 图片点击事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parent"></param>
        public void ImageTouched(object source, object parent)
        {
            var img = source as Image;
            var sp = parent as UniformGrid;
            if (img != null && img.Tag.ToString() == "0" && sp != null)
            {
                AdjustImageStaus(sp, img);
            }
        }

        /// <summary>
        /// 调整图片状态
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="img"></param>
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

        /// <summary>
        /// 清除选中状态
        /// </summary>
        /// <param name="source"></param>
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

        #endregion

        #region 图片列表导航事件

        public void CurrentImageChanged(object sender)
        {
            var listBox = (SurfaceListBox)sender;
            if (listBox.SelectedIndex == -1)
                return;

            CurrentImage = ((FileModel)listBox.SelectedItem).FileName;
        }

        public void Prev()
        {

        }

        public void Next()
        {

        }

        #endregion
       

        #region CLR 属性

        /// <summary>
        /// 可视元素个数
        /// </summary>
        private int _index = 3;

        private string _currentImage = @"/Images/Default.jpg";

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                NotifyOfPropertyChange(() => CurrentImage);
            }
        }

        #endregion

    }
}
