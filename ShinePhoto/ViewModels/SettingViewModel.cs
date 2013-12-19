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
    [Export(typeof(SettingViewModel))]
    public class SettingViewModel : Screen
    {
        protected override void OnActivate()
        {
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
                //NotifyOfPropertyChange(() => CanRegist);
            }
        }

        public string Folder
        {
            get { return _folder; }
            set
            {
                _folder = value;
                NotifyOfPropertyChange(() => Folder);
                //NotifyOfPropertyChange(() => CanRegist);
            }
        }

        public string WaterMarkImage
        {
            get { return _waterMarkImage; }
            set
            {
                _waterMarkImage = value;
                NotifyOfPropertyChange(() => WaterMarkImage);
                //NotifyOfPropertyChange(() => CanRegist);
            }
        }

        public string Logo
        {
            get { return _logo; }
            set
            {
                _logo = value;
                NotifyOfPropertyChange(() => Logo);
                //NotifyOfPropertyChange(() => CanRegist);
            }
        }

        #endregion


        #region 方法

        public void Go()
        { }

        public bool CanGo
        {
            get
            {
                if (string.IsNullOrEmpty(MainBackground) || string.IsNullOrEmpty(Folder) || string.IsNullOrEmpty(WaterMarkImage) || string.IsNullOrEmpty(Logo))
                {
                    _eventAggregator.Publish(new RegistNextEvent(new UserModel { }, false));
                    return false;
                }
                _eventAggregator.Publish(new RegistNextEvent(new UserModel { }, true));
                return true;
            }
        }


        #endregion

    }
}
