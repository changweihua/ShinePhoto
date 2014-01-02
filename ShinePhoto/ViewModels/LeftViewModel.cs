using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ShinePhoto.Events;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 左侧导航视图 ViewModel
    /// </summary>
    [Export(typeof(LeftViewModel))]
    public class LeftViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public LeftViewModel(AdViewModel adViewModel, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AdViewModel = adViewModel;
        }

        /// <summary>
        /// 广告视图
        /// </summary>
        public AdViewModel AdViewModel { get; private set; }

        /// <summary>
        /// 导航图片按钮单击事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parent"></param>
        public void ImageTouched(object source, object parent)
        {
            var img = source as Image;
            var sp = parent as StackPanel;
            if (img != null && sp != null)
            {
                AdjustImageStaus(sp, img);
                int number = Convert.ToInt32(img.Tag);
                _eventAggregator.Publish(new ModuleChangedEvent(null, img.Tag.ToString(), TurnNumberIntoType(number)));
            }
        }

        /// <summary>
        /// 根据按钮信息获取视图真实类型
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Type TurnNumberIntoType(int number)
        {
            Type type = null;

            switch (number)
            {
                case 0:
                    type = typeof(ShineDisplayViewModel);
                    break;
                case 1:
                    type = typeof(SignatureAlbumViewModel);
                    break;
                case 2:
                    type = typeof(CaptureViewModel);
                    break;
                default:
                    break;
            }        

            return type;
        }


        /// <summary>
        /// 调整图片按钮状态
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="img"></param>
        void AdjustImageStaus(StackPanel sp, Image img)
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
                        img.Source = new BitmapImage(new Uri(name + "_Selected" + ext, UriKind.RelativeOrAbsolute));
                        continue;
                    }

                    var fileName = image.Source.ToString().Replace("_Selected", "");
                    image.Source = new BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
                    
                }
            }
          
        }
    }
}
