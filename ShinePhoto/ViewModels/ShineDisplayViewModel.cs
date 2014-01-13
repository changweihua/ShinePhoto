using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Interface;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 炫影展示视图 ViewModel
    /// </summary>
    [Export(typeof(ShineDisplayViewModel))]
    public class ShineDisplayViewModel : Screen,IShellView
    {  
        #region 属性

        /// <summary>
        /// 程序品牌 Logo 高度
        /// </summary>
        private double _logoHeight = 120d;

        public double LogoHeight
        {
            get
            {
                return _logoHeight;
            }
            set
            {
                _logoHeight = value;
                NotifyOfPropertyChange(() => LogoHeight);
            }
        }

        #endregion
    }
}
