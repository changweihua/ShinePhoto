using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interface;
using System.ComponentModel.Composition;

namespace CMONO.Plugin.ViewModels
{
    [Export(typeof(TestViewModel))]
    public class TestViewModel : IShellView
    {
    }
}
