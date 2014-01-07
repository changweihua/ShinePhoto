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
    /// 签名相册视图 ViewModel
    /// </summary>
    [Export(typeof(SignatureAlbumViewModel))]
    public class SignatureAlbumViewModel : Screen,IShellView
    {
    }
}
