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
using System.Xml.Linq;
using ShinePhoto.Models;
using ShinePhoto.Views;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 左侧导航视图 ViewModel
    /// </summary>
    [Export(typeof(LeftViewModel))]
    public class LeftViewModel : Screen //Conductor<AdViewModel>.Collection.OneActive
    {
        private readonly IEventAggregator _eventAggregator;

        public BindableCollection<ModuleModel> ModuleModels { get; private set; }


        [ImportingConstructor]
        public LeftViewModel(AdViewModel adViewModel, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AdViewModel = adViewModel;
            ModuleModels = new BindableCollection<ModuleModel>();
        }

        public void LoadData(object source)
        {
            #region 加载组件

            var doc = XDocument.Load("config.xml");

            if (doc != null)
            {
                var nodes = doc.Root.Descendants("module").OrderBy((_) => { return Convert.ToInt32(_.Attribute("order").Value); });
                int index=0;
                foreach (var node in nodes)
                {
                    ModuleModels.Add(new ModuleModel { Icon = node.Attribute("icon").Value.ToString(), Tag = index++, ModuleName = node.Attribute("cname").Value.ToString() });
                    LogManager.GetLog(typeof(LeftViewModel)).Info(node.Attribute("name").Value.ToString());
                }

            }

            var view = source as ShinePhoto.Views.LeftView;

            ItemsControl navBar = null;
            if (view != null)
            {
                navBar = view.NavBar ;// ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<ItemsControl>(view, "NavBar");

                navBar.ItemsSource = ModuleModels;
            }


            #endregion
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
            var sp = parent as ItemsControl;
            if (img != null && sp != null)
            {
                AdjustImageStaus(sp, img);
                int number = Convert.ToInt32(img.Tag);
                _eventAggregator.Publish(new ModuleChangedEvent(null, img.Tag.ToString(), TurnNumberIntoType(number)));
            }
        }

        /// <summary>
        /// 导航图片按钮单击事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="topParent"></param>
        /// <param name="parent"></param>
        public void ImageTouched(object source, object topParent, object parent)
        {
            var img = source as Image;
            var ic = topParent as ItemsControl;
            var border = parent as Border;
            if (img != null && ic != null && border != null)
            {
                AdjustImageStaus(ic, border);
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
        void AdjustImageStaus(ItemsControl sp, Image img)
        {
            foreach (var item in ShinePhoto.Helpers.TreeHelper.GetChildObjects<Image>(sp, ""))
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

        /// <summary>
        /// 调整图片选中状态
        /// </summary>
        /// <param name="ic"></param>
        /// <param name="border"></param>
        void AdjustImageStaus(ItemsControl ic, Border border)
        {
            foreach (var item in ShinePhoto.Helpers.TreeHelper.GetChildObjects<Border>(ic, "").Skip(1))
            {
                if (item.GetType() == typeof(Border))
                {
                    var b = item as Border;

                    if (b == border)
                    {
                        b.BorderThickness = new System.Windows.Thickness(2);
                        continue;
                    }

                    b.BorderThickness = new System.Windows.Thickness(0);

                }
            }
        }
    }
}
