using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using ShinePhoto.Events;
using ShinePhoto.Models;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 设置 Screen ViewModel
    /// </summary>
    [Export(typeof(SettingViewModel))]
    public class SettingViewModel : Screen
    {
        public RegistViewModel ParentViewModel { get; set; }
        protected override void OnActivate()
        {
            var parent = ParentViewModel = (this.Parent as RegistViewModel);
            MainBackground = parent.User.MainBackground;
            Folder = parent.User.Folder;
            WaterMarkImage = parent.User.WaterMarkImage;
            Logo = parent.User.Logo;
            base.OnActivate();
        }

        #region 事件传递与获取

        private readonly IEventAggregator _eventAggregator;

        public SettingViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region 属性

        private string _mainBackground;
        private string _folder;
        private string _waterMarkImage;
        private string _logo;


        public string MainBackground
        {
            get { return _mainBackground; }
            set
            {
                _mainBackground = value;
                NotifyOfPropertyChange(() => MainBackground);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        public string Folder
        {
            get { return _folder; }
            set
            {
                _folder = value;
                NotifyOfPropertyChange(() => Folder);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        public string WaterMarkImage
        {
            get { return _waterMarkImage; }
            set
            {
                _waterMarkImage = value;
                NotifyOfPropertyChange(() => WaterMarkImage);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        public string Logo
        {
            get { return _logo; }
            set
            {
                _logo = value;
                NotifyOfPropertyChange(() => Logo);
                NotifyOfPropertyChange(() => CanGo);
            }
        }

        #endregion


        #region 方法

        public void Go()
        {
            _eventAggregator.Publish(new SettingEvent(new Setting { Folder = Folder, Logo = Logo, MainBackground = MainBackground, WaterMarkImage = WaterMarkImage }, true));
        }

        public bool CanGo
        {
            get
            {
                if (string.IsNullOrEmpty(MainBackground) || string.IsNullOrEmpty(Folder) || string.IsNullOrEmpty(WaterMarkImage) || string.IsNullOrEmpty(Logo))
                {
                    _eventAggregator.Publish(new SettingEvent(new Setting {  }, false));
                    return false;
                }
                _eventAggregator.Publish(new SettingEvent(new Setting { Folder = Folder, Logo = Logo, MainBackground = MainBackground, WaterMarkImage = WaterMarkImage }, true));
                return true;
            }
        }

        #endregion

    }
}
