using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 主视图 ViewModel
    /// </summary>
    [Export(typeof(AppViewModel))]
    public class AppViewModel : PropertyChangedBase
    {
       
    }
}
