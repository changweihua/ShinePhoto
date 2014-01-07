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
    }
}
