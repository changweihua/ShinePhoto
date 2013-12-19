using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(ShineDisplayViewModel))]
    public class ShineDisplayViewModel : IShellView
    {
    }
}
